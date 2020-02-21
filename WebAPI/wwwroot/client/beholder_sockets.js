const apiUrl = "http://localhost:50416/api";
var connection = null;
var userToken = "";

function appendMessage(message) {
    var ul = document.getElementById("messagesList");
    var li = document.createElement("li");
    li.appendChild(document.createTextNode(message));
    ul.appendChild(li);
}

document.getElementById("connectButton").addEventListener("click", function () {
    userToken = document.getElementById("userToken").value;
    if (userToken) {
        connection = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:50416/battlemap", { accessTokenFactory: () => userToken })
            .configureLogging(signalR.LogLevel.Information)
            .build();

        //You need to establish a connection using the accessTokenFactory as a function that gets the access token

        // Defined Operations
        connection.on("BAD_REQUEST", function () {
            appendMessage("BECEREMEDIK");
        });

        connection.on("USER_CONNECTED", function (message) {
            appendMessage(message);
        });

        connection.on("USER_DISCONNECTED", function (message) {
            appendMessage(message);
        });

        connection.on("USER_CONNECTED_ROOM", function (message) {
            appendMessage(message);
            console.log("User Room Fired");
        });

        connection.on("USER_DISCONNECTED_ROOM", function (message) {
            appendMessage(message);
        });

        connection.on("MESSAGE", function (message) {
            console.log("On Message");
            appendMessage(message);
        });

        connection.on("USER_CONNECTED_ROOM_ERROR", function (message) {
            appendMessage(message);
        });

        connection.on("USER_DISCONNECTED_ROOM_ERROR", function (message) {
            appendMessage(message);
        });

        connection.on("CREATE_ROOM", function (message) {
            appendMessage(message);
        });

        connection.on("CREATE_ROOM_ERROR", function (message) {
            appendMessage(message);
        });

        connection
            .start()
            .then(function () {
                appendMessage("connected");
                let roomName = "";

                document
                    .getElementById("joinRoomButton")
                    .addEventListener("click", function (event) {
                        event.preventDefault();
                        roomName = document.getElementById("roomName").value;
                        if (roomName) {
                            connection.invoke("JoinRoom", roomName).catch(function (err) {
                                return console.error(err);
                            });
                        } else {
                            alert("Please enter Room Name");
                        }
                    });

                document
                    .getElementById("createRoomButton")
                    .addEventListener('click', function (event) {
                        event.preventDefault();
                        userToken = document.getElementById("userToken").value;
                        if (userToken) {
                            connection.invoke("CreateRoom")
                                .then(message => {
                                    document.getElementById("tokenOutput").innerHTML = message;
                                    console.log(message);
                                })
                                .catch(function (err) {
                                    return console.error(err);
                                });
                        } else {
                            alert("Please Enter User Token");
                        }
                    });

                document
                    .getElementById("sendMessageButton")
                    .addEventListener('click', function (event) {
                        event.preventDefault();
                        const message = document.getElementById("messageInput").value;
                        console.log(message.value);
                        console.log(roomName.value);
                        connection.invoke("SendMessage", roomName, message).catch(function (err) {
                            return console.error(err);
                        });
                    });

                document
                    .getElementById("leaveRoomButton")
                    .addEventListener('click', function (event) {
                        event.preventDefault();
                        console.log(connection);
                        connection.invoke("LeaveRoom", roomName).catch(function (err) {
                            return console.error(err);
                        });
                    });
            })
            .catch(function () {
                appendMessage("error");
            });
    } else {
        alert("Please enter User Token");
    }
});