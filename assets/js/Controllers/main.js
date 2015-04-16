// assets/js/websockettester.js

// initialize global variables
var Handler;
var WebSocketClient;
var ViewHandler;
var Schools;

/**
 * set up the actions and global variables on the page
 */
$(document).ready(function () {
    // initialize the console and the Handler
    Console = new Console($("#console"));
    Handler = new WebSocketTestHandler(Console);

    //Create Views and show debug
    ViewHandler = new Views();
    ViewHandler.Add("debug", $("#debug"));
    ViewHandler.Add("schools", $("#schools"));
    ViewHandler.Add("roadConstruction", $("#roadConstruction"));
    ViewHandler.Show("debug");

    //Create elements
    Schools = new Schools($("#schoolTable"));
    Schools.Add("Stijn", "Test", "Test");
    Schools.Add("Stijn2", "Test2", "Test2");
    Schools.Remove(0);

    // assign event handler to the connect button
    $("#connect").click(function (e) {
        var server = Settings.URI;


        window.WebSocketClient = new WebSocketClient(server, window.Handler);

        e.preventDefault();
    });

    // assign event handler to the disconnect button
    $("#disconnect").click(function (e) {
        window.WebSocketClient.disconnect();
        e.preventDefault();
    });

    // assign event handler to the send button
    $("#send").click(function (e) {
        var message = $("#prompt").val();
        window.WebSocketClient.send(message);
        $("#prompt").val('');
        e.preventDefault();
    });
});