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
    ViewHandler.Add("dashboard", $("#dashboard"));
    ViewHandler.Add("debug", $("#debug"));
    ViewHandler.Add("schools", $("#schools"));
    ViewHandler.Add("roadConstructions", $("#roadConstructions"));
    ViewHandler.Add("zones", $("#zones"));
    ViewHandler.Show("debug");



    // assign event handler to the connect button
    $(".connect").click(function (e) {
        var server = Settings.URI;

        //if (typeof window.WebSocketClient.serverURI == "undefined") {
        //  console.log('true');
        window.WebSocketC = new WebSocketClient(server, window.Handler);
        //}

        e.preventDefault();
    });

    // assign event handler to the disconnect button
    $(".disconnect").click(function (e) {
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
        addToLatestCommands(message);
        $('#input .text_io').val('');
        window.commandIndex = getLocalCommands().length + 1;
        e.preventDefault();
    });

    $("#newZoneForm").submit(function (e) {
        e.preventDefault();
        $(".modal.in").modal('hide');
        window.WebSocketC.handler.newZone(new Zone($("input[name='zoneID']", this).val(), $("input[name='zoneName']", this).val()));

    });

    $("#newSchoolForm").submit(function (e) {
        e.preventDefault();
        $(".modal.in").modal('hide');
        window.WebSocketC.handler.newSchool(new School($("input[name='schoolID']", this).val(), $("input[name='schoolName']", this).val(), $("input[name='timeStart']", this).val(), $("input[name='timeEnd']", this).val()), 1);

    });

    $("#newRoadConstructionForm").submit(function (e) {
        e.preventDefault();
        $(".modal.in").modal('hide');
        window.WebSocketC.handler.newRoadConstruction(new RoadConstruction($("input[name='roadConstructionID']", this).val(), $("input[name='roadConstructionName']", this).val(), $("input[name='dateStart']", this).val(), $("input[name='dateEnd']", this).val()), 1);

    });



    $('.text_io').bind('keyup', function (e) {


        if (e.which == 13) {
            //enter
            $("#send").trigger("click");
        } else if (e.which == 38) {
            //omhoog
            LatestCommands = getLocalCommands();
            console.log(LatestCommands);
            if (typeof window.commandIndex == "undefined" || window.commandIndex < 0) {
                window.commandIndex = LatestCommands.length;
            } else {
                window.commandIndex--;
            }
            if (window.commandIndex == 0 || window.commandIndex > LatestCommands.length) {
                $(this).val("");
                window.commandIndex--;

            } else {
                $(this).val(LatestCommands[window.commandIndex - 1]);
            }
            console.log(window.commandIndex);

        } else if (e.which == 40) {
            //omlaag
            LatestCommands = getLocalCommands();
            console.log(LatestCommands);
            if (typeof window.commandIndex == "undefined" || window.commandIndex >= LatestCommands.length || window.commandIndex < 0) {
                $(this).val("");
                window.commandIndex = -1;

            } else {
                window.commandIndex++;
                $(this).val(LatestCommands[window.commandIndex - 1]);
            }
            console.log(window.commandIndex);
        }
    });


    function getLocalCommands() {
        var LatestCommands;
        if (localStorage.commands === null || localStorage.commands == "" || typeof localStorage.commands == "undefined") {
            LatestCommands = new Array();
            console.log("no storage set");
        } else {
            LatestCommands = JSON.parse(localStorage.commands);
        }
        return LatestCommands;
    }

    function addToLatestCommands(message) {
        message = $.trim(message);
        var LatestCommands = getLocalCommands();
        for (var key in LatestCommands) {
            if (LatestCommands[key] == message) {
                LatestCommands.splice(key, 1);
            }
        }
        LatestCommands.push(message);
        localStorage.commands = JSON.stringify(LatestCommands);
        //$(".latestCommands").prepend("<button type='button' class='btn btn-default'>" + message + "</button>");


        // on latest command click
        $(".latestCommands .btn").on("click", function () {
            window.WebSocketC.send($(this).text());
        });
    }


});