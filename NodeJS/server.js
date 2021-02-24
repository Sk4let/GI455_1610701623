var websocket = require("ws");
const sqlite = require('sqlite3').verbose();

var db = new sqlite.Database('./db/chatDB.db', sqlite.OPEN_CREATE | sqlite.OPEN_READWRITE, (err) => {

   if (err) throw err;

   console.log("Connected to database");
});

var callbackInitServer = () => {
   console.log("Server is running at port 9523");
}
var websocketServer = new websocket.Server({ port: 9523 }, callbackInitServer);

var clients = []; // Clients
var roomList = []; // Rooms

// roomList = {
//   roomName: "xx",
//   clients: []
//}


// Client connect to Server
websocketServer.on("connection", (ws, rq) => {
   console.log("==============================[ Client Connecting . . . ]====================================");
   clients.push(ws); // Send Client to clients[]

   {
      ws.on("message", (data) => {


         Boardcast(data);

         var toJsonObj = JSON.parse(data);

         var jsonObj = {
            eventName: toJsonObj.eventName,
            data: toJsonObj.data
         }

         console.log(jsonObj);

         var splitStr = jsonObj.data.split("#");
         console.log(splitStr);
         var userID = splitStr[0];
         var password = splitStr[1];
         var name = splitStr[2];

         var sqlSelect = "SELECT * FROM UserData WHERE UserID='"+userID+"' AND Password='"+password+"'"; //Login
         var sqlInsert = "INSERT INTO UserData (UserID, Password, Name) VALUES ('"+userID+"', '"+password+"', '"+name+"')" //Register


         //Register
         if( jsonObj.eventName == "Register")
         {
            db.all(sqlInsert, (err,rows)=>{
               if(err) 
               {
                  var callbackMsg = {
                     eventName:"Register",
                     data:"fail" + "#" + "User already in use"
                  }
                  var toJson = JSON.stringify(callbackMsg)
                  ws.send(toJson);
               }
               else 
               {
                  var callbackMsg = {
                     eventName:"Register",
                     data:"success" + "#" + "Register Success"
                  }
                  var toJson = JSON.stringify(callbackMsg)
                  ws.send(toJson);
               }
               
            });
         }
         else if (jsonObj.eventName == "Login")
         {
            db.all(sqlSelect, (err,rows)=>{
               if(err)
               {
                  var callbackMsg = {
                     eventName:"Login",
                     data:"fail"
                  }
                  var toJson = JSON.stringify(callbackMsg)
                  ws.send(toJson);
               }
               else 
               {
                  if (rows.length > 0)
                  {
                     console.log(rows);
                     var callbackMsg = {
                        eventName:"Login",
                        data:"success" + "#" + rows[0].Name
                     }
                     var toJson = JSON.stringify(callbackMsg)
                     ws.send(toJson);
                  }
                  else if (rows.length <= 0)
                  {
                     var callbackMsg = {
                        eventName:"Login",
                        data:"fail" + "#" + "not found"
                     }
                     var toJson = JSON.stringify(callbackMsg)
                     ws.send(toJson);
                  }
                 
               }

            });

            console.log("Clients Count : " + clients.length)

         }
         else if (jsonObj.eventName == "CreateRoom")
         {
            var isFoundRoom = false;
            for ( var i = 0; i < roomList.length; i++)
            {
               if(roomList[i].roomName == jsonObj.data)
               {
                  isFoundRoom = true;
                  break;
               }
            }
            if (isFoundRoom == true)
            {
               var callbackMsg = {
                  eventName:"CreateRoom",
                  data: "fail" + "#" + "room already exist"
               }
               var toJson = JSON.stringify(callbackMsg)
               ws.send(toJson);
            }
            else 
            {
               var newRoom = {
                  roomName: jsonObj.data,
                  clients: []
               }

               roomList.push(newRoom);
               roomList[i].clients.push(ws);


               var callbackMsg = {
                  eventName:"CreateRoom",
                  data: "success" + "#" + jsonObj.data
               }
               var toJson = JSON.stringify(callbackMsg)
               ws.send(toJson);
            }

         }
         else if (jsonObj.eventName == "JoinRoom")
         {
            var isClientFoundRoom = false;
            for(var i = 0; i < roomList.length; i++)
            {
               if(roomList[i].roomName == jsonObj.data)
               {
                  isClientFoundRoom = true;
                  break
               }
            }

            if(isClientFoundRoom)
            {
               var JoinData = {
                  roomName: jsonObj.data,
                  clients: []
               }

               roomList.push(JoinData);

               var callbackMsg = {
                  eventName:"JoinRoom",
                  data: "success" + "#" +jsonObj.data
               }
               var toJson = JSON.stringify(callbackMsg)
               ws.send(toJson);

            }
            else 
            {
               var callbackMsg = {
                  eventName:"JoinRoom",
                  data: "fail" + "#" + "Cannot join room"
               }
               var toJson = JSON.stringify(callbackMsg)
               ws.send(toJson);

            }
         }
         else if(jsonObj.eventName == "LeaveRoom")
         {
            var isLeaveSuccess = false;

            for (var i = 0; i < roomList.length; i++)
            {
               for (var j = 0; j < roomList[i].clients.length; j++)
               {
                  if(ws == roomList[i].clients[j])
                  {
                     roomList[i].clients.splice(j, 1);

                     if (roomList[i].clients.length <= 0)
                     {
                        roomList.splice(i, 1);
                     }
                     isLeaveSuccess = true;
                     break;
                  }
               }
            }

            if(isLeaveSuccess)
            {
               var callbackMsg = {
                  eventName:"LeaveRoom",
                  data: "success" + "#" + "leave room success"
               }
               var toJson = JSON.stringify(callbackMsg)
               ws.send(toJson);
            }
            else 
            {
               var callbackMsg = {
                  eventName:"LeaveRoom",
                  data: "fail" + "#" + "cannot leave room"
               }
               var toJson = JSON.stringify(callbackMsg)
               ws.send(toJson);
            }

          
         }
         else if(jsonObj.eventName == "Message")
         {
            var callbackMsg = {
               eventName:"Message",
               data: splitStr[0] + "#" + splitStr[1] + "#" + splitStr[2] + "#" + splitStr[3]
            }
            var toJson = JSON.stringify(callbackMsg)
            ws.send(toJson);
         }
         
      });
   }


   // If client logout
   ws.on("close", () => {
      console.log("client disconnected.");

      for (var i = 0; i < roomList.length; i++)
       {
           for (var j = 0; j < roomList[i].clients.length; j++)
         {
            if(ws == roomList[i].clients[j])
             {
                roomList[i].clients.splice(j, 1);

                if (roomList[i].clients.length <= 0)
                {
                   roomList.splice(i, 1);
                }

                break;
             }
          }
       }
   });
});

// send Data to clients
function Boardcast(data) {

   for ( var i = 0 ; i < clients.length; i++)
   {
      clients[i].send(data);
   }
}