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
    this.SchoolList;
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
    setTimeout(this.getData(), 2000);
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

WebSocketHandler.prototype.getData = function () {

    //Ask for all schools
    for (var key in this.ZoneList.ZoneList) {
        console.log('school');
        this.send(">CZON:" + this.ZoneList.ZoneList[key].ID + ":5:5:;")
        this.send(">GETS:" + this.ZoneList.ZoneList[key].ID + ":;"); //Get Schools 
    };
};