var websocket = require("ws");

var callbackInitServer = () => {
   console.log("Server is running");
}
var websocketServer = new websocket.Server({ port: 9523 }, callbackInitServer);

var clients = [];

websocketServer.on("connection", (ws, rq) => {
   clients.push(ws);

   ws.on("message", (data) => {
      //#register#I want my Id
      var ext = data.split("#"); // ["","register","I want my Id"]
      var id = ext[0]; // ""
      var type = ext[1]; // register
      var c_data = ext[2]; // I want my Id

      console.log(`ข้อมูลที่ Client ส่งเข้ามา`);
      console.log(ext);

      // check type
      switch (type) {
         case "register": // ส่งมาจาก Start()

            // จัดเตรียมข้อมูล
            ws["client_id"] = clients.length;
            // ส่งข้อมูลไปที่ Client
            ws.send(`${ws["client_id"]}#${type}#${"Register Completed"}`)
            console.log(`ข้อมูลที่ Server ส่งให้ Client `);
            console.log(`${ws["client_id"]}#${type}#${"Register Completed"}`)
            break;
            
         case "message":
            boardcast(`${id}#${type}#${c_data}`)
            console.log(`${id}#${type}#${c_data}`);
            break;
         default:
            break;
      }

   });

   ws.on("close", () => {
      for (var i = 0; clients.length; i++) {
         if (clients[i] == ws) {
            clients.splice(i, 1);
            break;
         }
      }
   });
});

function boardcast(data) {
   for (var i = 0; i < clients.length; i++) {
      clients[i].send(data);
   }
}
