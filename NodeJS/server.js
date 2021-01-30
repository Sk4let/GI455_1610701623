var websocket = require('ws');

var websocketServer = new websocket.Server({port:25500}, ()=>{
      console.log("Server is running");
});

var wsList = [];

websocketServer.on("connection", (ws, rq)=>{
    console.log('client connected.');

    wsList.push(ws);

    ws.on("message", (data)=>{
        console.log("send from client :"+data);
        Boardcast(data);
    });

    ws.on("close", ()=>{
        
      // wsList = ArrayRemove(wsList, ws);

        for(var i =0; i < wsList.length; i++)
        {
            if(wsList[i] == ws)
            {
                wsList.splice(i,1);
                break;
            }
        }
        console.log("client disconnected.");
        console.log(wsList.length);
    });
});

/*function ArrayRemove(arr, value)
{
  return arr.filter((element)=>{
        return element != value;
  })
}*/

function Boardcast(data)
{
    for(var i = 0; i < wsList.length; i++)
    {
        wsList[i].send(data);
    }
}

