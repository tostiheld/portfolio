// assets/js/websockettester.js

// initialize global variables
var Handler;
var WebSocketC;
var ViewHandler;
var Schools;
var RoadConstructions;
var Zones;

/**
 * set up the actions and global variables on the page
 */
$(document).ready(function () {
    // initialize the console and the Handler
    Console = new Console($("#console"));
    Handler = new WebSocketHandler(Console);

    //Create Views and show debug
    ViewHandler = new Views();
    ViewHandler.Add("debug", $("#debug"));
    ViewHandler.Add("schools", $("#schools"));
    ViewHandler.Add("roadConstructions", $("#roadConstructions"));
    ViewHandler.Add("zones", $("#zones"));
    ViewHandler.Show("debug");


    Handler.ZoneList = new Zones($("#zoneTable"));


    // assign event handler to the connect button
    $("#connect").click(function (e) {
        var server = Settings.URI;

        //if (typeof window.WebSocketClient.serverURI == "undefined") {
        //  console.log('true');
        window.WebSocketC = new WebSocketClient(server, window.Handler);
        //}

        e.preventDefault();
    });

    // assign event handler to the disconnect button
    $("#disconnect").click(function (e) {
        window.Handler.onDisconnect();
        window.WebSocketC.disconnect();
        window.WebSocketC == null;
        e.preventDefault();
    });

    // assign event handler to the send button
    $("#send").click(function (e) {
        var message = $("#prompt").val();
        window.WebSocketC.send(message);
        $("#prompt").val('');
        e.preventDefault();
    });


});