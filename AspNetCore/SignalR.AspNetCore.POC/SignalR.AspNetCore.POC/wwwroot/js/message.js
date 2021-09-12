"use strict";

const TOKEN_FIREFOX = "INFORM THE TOKEN";

const TOKEN_CHROME = "INFORM THE TOKEN";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost/SignalR.AspNetCore.POC/messages", {
        accessTokenFactory: () => (navigator.userAgent.indexOf("Chrome") !== -1) ? TOKEN_CHROME : TOKEN_FIREFOX
    })
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.on("ReceiveMessage", function (message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&alt;").replace(/>/g, "&gt;");
    var div = document.createElement("div");
    div.innerHTML = msg + "<hr />";
    document.getElementById("messages").appendChild(div);
});

connection.on("UserConnected", function (connectionId) {
    var groupElement = document.getElementById("group");
    var option = document.createElement("option");
    option.text = connectionId;
    option.value = connectionId;
    groupElement.add(option);
});

connection.on("UserDisconnected", function (connectionId) {
    var groupElement = document.getElementById("group");
    for (var i = 0; i < groupElement.length; i++) {
        if (groupElement.options[i].value == connectionId) {
            groupElement.remove(i);
        }
    }
});

connection.start().catch(function (error) {
    return console.error(error.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("message").value;

    var groupElement = document.getElementById("group");
    var groupValue = groupElement.options[groupElement.selectedIndex].value;

    if (groupValue == "mySelf" || groupValue == "all") {
        var method = groupValue == "mySelf" ? "SendMessageToCaller" : "SendMessageToAll";

        connection.invoke(method, message).catch(function (error) {
            return console.error(error.toString());
        });
    } else if (groupValue == "privateGroup") {
        connection.invoke("SendMessageToGroup", "privateGroup", message).catch(function (error) {
            return console.error(error.toString());
        });
    } else {
        connection.invoke("SendMessageToUser", groupValue, message).catch(function (error) {
            return console.error(error.toString());
        });
    }

    event.preventDefault();
});

document.getElementById("joinGroup").addEventListener("click", function (event) {
    connection.invoke("JoinGroup", "privateGroup").catch(function (error) {
        return console.error(error.toString());
    });

    event.preventDefault();
});