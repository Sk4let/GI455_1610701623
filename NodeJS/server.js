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

      console.log("send from client : "+data);
      Boardcast(data);

   });
   // If client logout
   ws.on("close", () => {
      for (var i = 0; clients.length; i++) {
         if (clients[i] == ws) {
            clients.splice(i, 1);
            break;
         }
      }
      console.log("client disconnected.");
   });
});

// send Data to clients
function Boardcast(data) {
   for (var i = 0; i < clients.length; i++) {
      clients[i].send(data);
   }
}
