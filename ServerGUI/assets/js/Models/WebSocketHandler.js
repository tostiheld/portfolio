// assets/js/WebSocketHandler.js

/**
 *  This event handler responds to the WebSocketClient, customizing the actions we take
 *  when it issues events such as onOpen or onMessage.
 *
 *  We implement this separately so that we can perform layout-specific changes while
 *  keeping the WebSocketClient abstracted from UI
 */
function WebSocketHandler(console) {
    this.con = console;
    this.WebSocketC;
    this.ZoneList;
}

/**
 *  when the user tries to send a message,
 *  publish your message into the console
 */
WebSocketHandler.prototype.send = function (message) {
    this.con.appendFromClient(message);
    this.WebSocketC.ws.send(message);
}

/**
 * upon connection,
 *   - exchange the Connect button for a Disconnect button,
 *   - allow user to send messages to the server,
 *   - notify user of the connection
 */
WebSocketHandler.prototype.onOpen = function () {
    $("#connected").html('<i class="icon-check"></i>');
    $("#connect").hide();
    $("#disconnect").show();
    $("#send").removeClass("disabled");
    this.send(">IDEN:UI:;");
    this.newZone(new Zone(1, "test", 5, 8));
    this.newZone(new Zone(2, "test2", 1, 10));
    this.getData();
};
/**
 *  when a message is recieved from the server,
 *  publish the server's message into the console
 */
WebSocketHandler.prototype.onMessage = function (e) {
    var message = e.data;
    this.con.appendFromServer(message);

};

/**
 * upon connection,
 *   - exchange the Connect button for a Disconnect button,
 *   - allow user to send messages to the server,
 *   - notify user of the connection
 */
WebSocketHandler.prototype.onClose = function () {
    $("#connected").html('<i class="icon-check-empty"></i>');
    $("#disconnect").hide();
    $("#connect").show();
    $("#send").addClass("disabled");
};

/**
 * if there was an error,
 * print the error into the console
 */
WebSocketHandler.prototype.onError = function (e) {
    var message = e.data;
    this.con.appendError(message);
};

WebSocketHandler.prototype.onDisconnect = function () {
    this.send(">DISC:;");
}

WebSocketHandler.prototype.newZone = function (newZone) {
    this.ZoneList.Add(newZone);
    this.send(">CZON:" + newZone.ID + ":" + newZone.X + ":" + newZone.Y + ":;");
    var wsh = this;
    $("tr .remove", this.Element).on('click', function () {
        wsh.removeZone($(this).parents("tr").attr("id"));
    });
}
WebSocketHandler.prototype.removeZone = function (id) {
    if (this.ZoneList.Remove(id)) {
        this.send(">RZON:" + id + ":;");
        console.log("remove zone: " + id);
    } else {
        console.log("could not be deleted");
    }
}

WebSocketHandler.prototype.getData = function () {
    //Ask for all Zones
    console.log('Get All Zones');
    this.send(">GETZ:;");

    //Ask for all schools,roadConstructions,Vertexes en Edges
    for (var key in this.ZoneList.ZoneList) {
        console.log('Get Schools for Zone:' + this.ZoneList.ZoneList[key].ID);
        this.send(">GETS:" + this.ZoneList.ZoneList[key].ID + ":;");

        console.log('Get RoadConstructions for Zone:' + this.ZoneList.ZoneList[key].ID);
        this.send(">GETR:" + this.ZoneList.ZoneList[key].ID + ":;");

        console.log('Get Vertexes for Zone:' + this.ZoneList.ZoneList[key].ID);
        this.send(">GETV:" + this.ZoneList.ZoneList[key].ID + ":;");

        console.log('Get Edges for Zone:' + this.ZoneList.ZoneList[key].ID);
        this.send(">GETE:" + this.ZoneList.ZoneList[key].ID + ":;");
    };


};