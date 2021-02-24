const socket = require('ws')
const server = new socket.Server({ port: 8080 })

let rooms = []

server.on("connection", (myself) => {


   myself.on("message", (json) => {
      const object = JSON.parse(json)
      const my_room = ""

      switch (object.eventName) {
         case "createRoom":
            rooms.forEach(function each(room) {
               if (room.roomName == object.data) {

                  myself.send(JSON.stringify({
                     eventName: "createRoom",
                     data: "400"
                  }))
               }
               else {
                  rooms.push({ roomName: object.data, clients: new Set([myself]) })
                  my_room = object.data
                  myself.send(JSON.stringify({
                     eventName: "createRoom",
                     data: "200"
                  }))
               }
            })

            break;
         case "joinRoom":
            rooms.forEach(function each(room) {
               if (room.roomName == object.data) {

                  room.clients.add(myself)
                  my_room = object.data
                  myself.send(JSON.stringify({
                     eventName: "joinRoom",
                     data: "200"
                  }))
               }
               else {
                  myself.send(JSON.stringify({
                     eventName: "joinRoom",
                     data: "400"
                  }))
               }
            })

            break;
         case "sendMessage":
            const split = object.data.split("|")
            rooms.forEach(function each(room){
               if(room.roomName == split[0])
               {
                  room.clients.forEach(function each (client){
                     if (client != myself)
                     {
                        client.send(JSON.stringify({
                           eventName: "sendMessage",
                           data: split[1]
                        }))
                     }
                  })
               }
            })

            break;
      }

   })
   myself.on("close", (code,reason)=>{
      if(my_room != "")
      {
         rooms.forEach(function each(room){
            if(room.roomName == my_room)
            {
               room.clients.delete(myself)
            }
         })
      }

      
   })

})
