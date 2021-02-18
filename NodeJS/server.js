var websocket = require("ws");

var callbackInitServer = () => {
   console.log("Server is running at port 9523");
}
var websocketServer = new websocket.Server({ port: 9523 }, callbackInitServer);

var clients = []; // Clients
var roomList = []; // Rooms

/*
{
   roomName: "xxxxxxxx",
   clients: []
}
*/


// Client connect to Server
websocketServer.on("connection", (ws, rq) => {

   {
      //LobbyZone

      ws.on("message", (data) => {

         console.log(data);

         var toJson = JSON.parse(data);

         //console.log(toJson.eventName);

         //CreateRoom
         if (toJson.eventName == "CreateRoom")
         {
            var isFoundRoom = false;
            for(var i=0;i < roomList.length; i++)
            {
               if(roomList[i].roomName == toJson.data)
               {
                  isFoundRoom = true;
                  break;
               }

            }

            if(isFoundRoom)
            {
               //Callback to client : roomName is exist.
               console.log("Create room : room is found");

               var resultData = {
                  eventName: toJson.eventName,
                  data: "fail"
               }

               var toJsonStr = JSON.stringify(resultData)

               ws.send(toJsonStr);
            }
            else 
            {
               //Create room here.
               console.log("Create room : room is not found");

               var newRoom = {
                  roomName : toJson.data,
                  clients: []
               }

               roomList.push(newRoom);

               newRoom.clients.push(ws);


               var resultData = {
                  eventName: toJson.eventName,
                  data: "success"
               }

               var toJsonStr = JSON.stringify(resultData)

               ws.send(toJsonStr);

            }
         }
         else if(toJson.eventName == "JoinRoom")//JoinROom
         {
            console.log("client request JoinRoom");
         }
         else if(toJson.eventName == "LeaveRoom")
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
               ws.send("LeaveRoomSuccess");

               console.log("leave room success");
            }
            else 
            {
               ws.send("LeaveRoomFail");

               console.log("leave room fail");
            }
         }
      });

   }

   console.log("client connected.");
   clients.push(ws); // Send Client to clients[]

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
   for (var i = 0; i < clients.length; i++) {
      clients[i].send(data);
   }
}
