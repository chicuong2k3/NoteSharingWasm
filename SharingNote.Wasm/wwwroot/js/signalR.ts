//import * as signalR from "@microsoft/signalr";

//// tạo connection
//const connection = new signalR.HubConnectionBuilder()
//    .withUrl("/hubs/view", { transport: signalR.HttpTransportType.LongPolling })
//    .build();

//function notify() {
//    connection.send("NotifyWatching");
//}

//connection.on("viewCount", (value: number) => {
//    const counter = document.getElementById("viewCounter");
//    counter.innerText = value.toString();
//});


//connection.start()
//    .then(() => {
//        console.log("Connected to server.");
//        notify();
//    }, () => {
//        console.log("Connection failed.");
//    })
//    .catch(err => console.error(err.toString()));