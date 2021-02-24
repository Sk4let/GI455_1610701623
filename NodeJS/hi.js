let rooms = []

let ws = {
   send: function(message) 
   {
      console.log(message);
   }
}

let rooms =  [
         {roomName: "Room 39", clients:[ws, ws, ws, ws, ws]},
         {roomName: "Room ABC", clients: [ws, ws]}, 
         {roomName: "Room 5555", clients: [ws, ws, ws]}
 ]

 // for each
 rooms