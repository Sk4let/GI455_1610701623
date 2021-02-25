const WebSocket = require('ws');

const ws = new WebSocket('wss://gi455-305013.an.r.appspot.com/');

const object = { eventName: "GetStudentData", studentID: "1610701623" }

ws.on('open', function open() {
    ws.send(JSON.stringify(object));
});

ws.on('message', function incoming(data) {
    console.log(data);
});