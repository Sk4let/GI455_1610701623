var websocket = require("ws");

var callbackInitServer = ()=>{
      console.log("Server is running");
}
var websocketServer = new websocket.Server({port:25565}, callbackInitServer);

var websocketList = [];

websocketServer.on("connection",(ws,rq)=>{
      console.log("Client Connected.");
      websocketList.push(ws);
         console.log(websocketList.length + " in Server");

      ws.on("message",(data)=>{
         console.log("send from client :" +data);
         Boardcast(data);
      });

      ws.on("close",()=>{
         for(var i = 0; websocketList.length; i++)
         {
            if (websocketList[i] == ws)
            {
               websocketList.splice(i,1);
               break;
            }
         }
         console.log("client disconnected.");
         console.log(websocketList.length + " in Server");
});
});

function Boardcast(data)
{
   for (var i = 0; i < websocketList.length; i++)
   {
      websocketList[i].send(data);
   }
}
