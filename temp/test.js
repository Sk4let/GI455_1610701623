let ws = {
   send: function (message) {
      console.log(message);
   }
}

let rooms = [
   { roomName: "Room 39", clients: new Set () },
   { roomName: "Room ABC", clients: new Set () },
   { roomName: "Room 5555", clients: new Set () }
]

//1. create room
rooms.push({ roomName: "Room NEW 4", clients: new Set () })

//2. join room
rooms.forEach(function each(room){
   console.log(room.roomName);
   if (room.roomName == "Room ABC")
   {
      room.clients.add(ws)
   }

})

console.log(rooms[1].clients.size)

// 3. Leave Room
rooms.forEach(function each(room){
   if (room.roomName == "Room ABC")
   {
      room.clients.delete(ws)
   }
})

console.log(rooms[1].clients.size)

// 4. Send Message
rooms.forEach(function each(room){
   if(room.roomName == "Room ABC")
   {
      room.clients.forEach(function each(client){
         if (client != ws)
         {
            client.send("55555")
         }
      })
   }
})

// JSON to Js object

const json = {"eventName":"createroom", "data":"Room 39"}

const object = JSON.parse(json)

console.log(object.eventName)
comsole.log(object.data)
