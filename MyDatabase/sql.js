const sqlite = require("sqlite3").verbose();

let db = new sqlite.Database('./db/chatDB.db', sqlite.OPEN_CREATE | sqlite.OPEN_READWRITE, (err)=>{

   if(err) throw err;

   console.log("Connected to database.");

   var dataFromClient = {
      eventName:"login",
      data:"test2222#100"
   }

   var splitStr = dataFromClient.data.split('#');

   var userID = splitStr[0];
   var addedMoney = parseInt(splitStr[1]);
   //var password = splitStr[1];
   //var name = splitStr[2];

   //var sqlSelect = "SELECT * FROM UserData WHERE UserID='"+userID+"' AND Password='"+password+"'"; //Login
   //var sqlInsert = "INSERT INTO UserData (UserID, Password, Name, Money) VALUES ('"+userID+"','"+password+"','"+name+"','0')"; //Register
   var sqlUpdate = "UPDATE UserData SET Money='200' WHERE UserID='"+userID+"'";

   db.all("SELECT Money FROM UserData WHERE UserID='"+userID+"'", (err,rows)=>{
      if (err)
      {
         var callbackMsg = {
            eventName:"AddMoney",
            data:"fail"
         }

         var toJson = JSON.stringify(callbackMsg);
         console.log("[0]" +toJson);
      }
      else 
      {
         console.log(rows);
         if (rows.length > 0)
         {
            var currentMoney = rows[0].Money;
            currentMoney += addedMoney;

            db.all("UPDATE UserData SET Money='"+currentMoney+"' WHERE UserID='"+userID+"'");
               if (err)
               {
                  var callbackMsg = {
                     eventName:"AddMoney",
                     data:"fail"
                  }
         
                  var toJson = JSON.stringify(callbackMsg);
                  console.log("[1]" +toJson);

               }
               else 
               {
                  var callbackMsg = {
                     eventName:"AddMoney",
                     data:currentMoney
                  }
         
                  var toJson = JSON.stringify(callbackMsg);
                  console.log("[0]" +toJson);
               }
         }
         else 
         {
            var callbackMsg = {
               eventName:"AddMoney",
               data:"fail"
            }
   
            var toJson = JSON.stringify(callbackMsg);
            console.log("[2]" +toJson);
         }

      }
   });
   
   /*db.all(sqlInsert, (err,rows)=>{
       if(err)
       {
          var callbackMsg = {
             eventName:"Register",
             data:"fail"
          }

          var toJson = JSON.stringify(callbackMsg);
          console.log("[0]"+toJson);
       }
       else 
       {
         var callbackMsg = {
            eventName:"Register",
            data:"success"
         }

         var toJson = JSON.stringify(callbackMsg);
         console.log("[1]"+toJson);
       }
   });*/

   /*db.all(sqlSelect, (err,rows)=>{
      if (err)
      {
         console.log("[0]" + err);
      }
      else 
      {
         if(rows.length > 0)
         {
            console.log("=======[1]========");
            console.log(rows);
            console.log("=======[1]========");
            var callbackMsg = {
               eventName:"login",
               data:rows[0].Name
            }

            var toJson = JSON.stringify(callbackMsg);
            console.log("[2]" +toJson);
         }
         else 
         {
            var callbackMsg = {
               eventName:"login",
               data:"fail"
            }

            var toJson = JSON.stringify(callbackMsg);
            console.log("[3]" +toJson);
            
         }
      }
   });*/
});