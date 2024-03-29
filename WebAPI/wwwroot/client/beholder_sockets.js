﻿function appendMessage(message) {
    var ul = document.getElementById("messagesList");
    var li = document.createElement("li");
    li.appendChild(document.createTextNode(message));
    ul.appendChild(li);
}

document.getElementById("loginButton").addEventListener("click", function () {
    var url = "/api/auth/login";

    var data = {};
    data.email = document.getElementById("email").value;
    data.password = document.getElementById("password").value;
    var json = JSON.stringify(data);

    var xhr = new XMLHttpRequest();
    xhr.open("POST", url, true);
    xhr.setRequestHeader('Content-type', 'application/json; charset=utf-8');
    xhr.onload = function () {
        if (xhr.readyState == 4 && xhr.status == "200") {
            var token = JSON.parse(xhr.responseText) && JSON.parse(xhr.responseText).token;
            localStorage.setItem("access_token", token);
            alert("Logged In.");
        } else {
            var err = JSON.parse(xhr.responseText);
            console.log("Error: " + err);
        }
    }
    xhr.send(json);
});

document
    .getElementById("createRoomButton")
    .addEventListener('click', function (event) {
        event.preventDefault();
        var url = "/api/sockets/rooms";

        var data = {};
        data.roomName = document.getElementById("roomName").value;
        data.description = document.getElementById("description").value;
        var json = JSON.stringify(data);

        var xhr = new XMLHttpRequest();
        xhr.open("POST", url, true);
        xhr.setRequestHeader('Content-type', 'application/json; charset=utf-8');
        xhr.setRequestHeader('Authorization', 'Bearer ' + localStorage.getItem('access_token'));
        xhr.onload = function () {
            if (xhr.readyState == 4 && xhr.status == "200") {
                var roomName = JSON.parse(xhr.responseText) && JSON.parse(xhr.responseText);
                appendMessage(roomName);
            } else {
                var err = JSON.parse(xhr.responseText);
                console.log("Error: " + err);
            }
        }
        xhr.send(json);
    });

document.getElementById("connectButton").addEventListener("click", function () {
    if (localStorage.getItem("access_token")) {
        var connection = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:50416/battlemap", {
                accessTokenFactory: () => localStorage.getItem("access_token")
            })
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
                let roomId = "";

                document
                    .getElementById("joinRoomButton")
                    .addEventListener("click", function (event) {
                        event.preventDefault();
                        roomId = document.getElementById("roomId").value;
                        if (roomId) {
                            connection.invoke("JoinRoom", roomId).catch(function (err) {
                                return console.error(err);
                            });
                        } else {
                            alert("Please enter Room Name");
                        }
                    });

                document
                    .getElementById("sendMessageButton")
                    .addEventListener('click', function (event) {
                        event.preventDefault();
                        const message = document.getElementById("messageInput").value;
                        console.log(message.value);
                        console.log(roomName.value);
                        connection.invoke("SendMessage", roomId, message).catch(function (err) {
                            return console.error(err);
                        });
                        document.getElementById("messageInput").value = "";
                    });

                document
                    .getElementById("leaveRoomButton")
                    .addEventListener('click', function (event) {
                        event.preventDefault();
                        roomName = document.getElementById("roomName").value;
                        connection.invoke("LeaveRoom", roomId).catch(function (err) {
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