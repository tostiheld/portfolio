// initialize global variables
var Handler;
var WebSocketC;
var ViewHandler;
var Schools;
var RoadConstructions;
var Zones;
var Console;

/**
 * set up the actions and global variables on the page
 */
$(document).ready(function () {
    // initialize the console and the Handler
    Console = new Console($("#console"));
    Handler = new WebSocketHandler(Console);

    //Create Views and show debug
    ViewHandler = new Views();
    ViewHandler.Add("dashboard", $("#dashboard"));
    ViewHandler.Add("debug", $("#debug"));
    ViewHandler.Add("schools", $("#schools"));
    ViewHandler.Add("roadConstructions", $("#roadConstructions"));
    ViewHandler.Add("zones", $("#zones"));
    ViewHandler.Show("debug");



    // assign event handler to the connect button
    $(".connect").click(function (e) {
        window.WebSocketC = new WebSocketClient(Settings.URI, window.Handler);
        e.preventDefault();
    });

    // assign event handler to the disconnect button
    $(".disconnect").click(function (e) {
        window.WebSocketC.disconnect();
        window.WebSocketC == null;
        e.preventDefault();
    });

    // assign event handler to the send button
    $("#send").click(function (e) {
        var message = $("#prompt").val();
        window.WebSocketC.send(message);
        $("#prompt").val('');
        Console.addToLatestCommands(message);
        $('#input .text_io').val('');
        window.commandIndex = Console.getLocalCommands().length + 1;
        e.preventDefault();
    });



    //
    // HANDLE KEY EVENTS OF CONSOLE
    //
    $('.console_io').bind('keyup', function (e) {
        Console.handleKeyEvents(e, this);
    });



});

function selectPort(zone) {
    window.Handler.send(">GET:ports:;");
    $('#portsModal').modal('show');
    $('#portsModal input[name="id"]').val(zone.ID);
}

//This is a custom method to log messages.
//The idea is that logging can be enabled/disabled
//nex to that is dl shorter than console.log
function dl(message) {
    //console.log(message);
}

//This method clears all element in a given element
function clearForm(formElement) {
    $(formElement).find("input, textarea, select").val("");
}