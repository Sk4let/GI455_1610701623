var websocket = require("ws");

var callbackInitServer = () => {
   console.log("Server is running");
}
var websocketServer = new websocket.Server({ port: 9523 }, callbackInitServer);

var clients = []; // Clients
// Client connect to Server
websocketServer.on("connection", (ws, rq) => {
   clients.push(ws); // Send Client to clients[]

   ws.on("message", (data) => {
      //#checkfirst#Login as guest
      var ext = data.split("#"); // split to ["","Checkfirst","Login as guest"]
      var id = ext[0]; // "" Blank/
      var type = ext[1]; // Checkfirst/
      var msg_data = ext[2]; // Login as guest/

      console.log("Client Send to Server");
      console.log("DATA is " +ext);
      console.log(" " +clients.length + "  in server");

      // Check Type or ext[1]
      switch (type) {
         case "register": // Send from Client at SendAction()
            // Assign Data between "client_id" with client in clients[]
            ws["client_id"] = clients.length;
            // Send to Client/
            // $ is string type but can assign values/
            // # is symbol for split data

            // After Assign new Data send data back to client
            ws.send(`${ws["client_id"]}#${type}#${"Login Completed"}#${clients.length}`)
            console.log("Server send data to client");
            // EX. 1#register#Login Completed
            console.log(`${ws["client_id"]}#${type}#${"Login Completed"}#${"In server " +clients.length}`)
            break;
            
         case "message": // Send from Client at SendAction
            boardcast(`${id}#${type}#${msg_data}`) //Send data to clients Ex. 
            console.log(`DATA is ${id}#${type}#${msg_data}`);
            break;
         default:
            break;
      }

   });
   // If client logout
   ws.on("close", () => {
      for (var i = 0; clients.length; i++) {
         if (clients[i] == ws) {
            clients.splice(i, 1);
            break;
         }
      }
   });
});

// send Data to clients
function boardcast(data) {
   for (var i = 0; i < clients.length; i++) {
      clients[i].send(data);
   }
}
