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
    this.Zones;
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
    $(".connect").hide();
    $(".disconnect").show();
    $("#send").removeClass("disabled");
    this.send(">IDEN:UI:;");
    this.Zones = new Zones($("#zoneTable"));
    this.newZone(new Zone(1, "test"));
    this.newZone(new Zone(2, "test2"));
    this.newSchool(new School("12", "Fontys", "08:30", "15:30"), 1);
    this.newRoadConstruction(new RoadConstruction("Road Construction 1", "01-05-2015", "05-05-2015", "5"), 1);
    //this.getData();
};
/**
 *  when a message is recieved from the server,
 *  publish the server's message into the console
 */
WebSocketHandler.prototype.onMessage = function (e) {
    var message = e.data;
    this.con.appendFromServer(message);
    try {
        var json = JSON.parse(message);
    } catch (e) {
        console.log("this is no json");
    }
    //if ports
    if (json['payloadtype'] == "ports") {
        var ports = json['payload'];
        for (var pkey in ports) {
            $(".arduinoPorts").append("<option>" + ports[pkey] + "</option>");
        }
    }


    //if temperature
    if (json['payloadtype'] == "temperature") {
        var minT = -30;
        var maxT = 50;
        var tempT = json['metadata'][0] + -(minT);
        var percentT = tempT / (maxT + -(minT)) * 100;
        $(".temp").css("width", percenT + "%");
    }
    console.log("message type:" + json['type'] + ", Message: " + json['metadata']);
};

/**
 * upon connection,
 *   - exchange the Connect button for a Disconnect button,
 *   - allow user to send messages to the server,
 *   - notify user of the connection
 */
WebSocketHandler.prototype.onClose = function () {
    $(".connect").show();
    $(".disconnect").hide();
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
    this.removeAll();
}

WebSocketHandler.prototype.newZone = function (newZone) {
    this.Zones.Add(newZone);
    if (typeof newZone.X == "undefined") {
        var x = "0";
    } else {
        x = newZone.X;
    }
    if (typeof newZone.Y == "undefined") {
        var y = "0";
    } else {
        var y = newZone.Y;
    }
    this.send(">Create:zone:" + newZone.ID + ":" + x + ":" + y + ":;");
    var wsh = this;
    $("tr .remove", this.Zones.Element).on('click', function () {
        wsh.removeZone($(this).parents("tr").attr("id"));
    });
}
WebSocketHandler.prototype.removeZone = function (id) {
    if (this.Zones.Remove(id)) {
        this.send(">Remove:zone:" + id + ":;");
        console.log("remove zone: " + id);
    } else {
        console.log("could not be deleted");
    }
    //TODO: delete all schools and roadConstructions linked to this Zone
}

WebSocketHandler.prototype.newSchool = function (school, zoneID) {
    for (var skey in this.Zones.ZoneList) {
        if (this.Zones.ZoneList[skey].ID == zoneID) {
            this.Zones.ZoneList[skey].SchoolList.Add(school);
            var schoolsTable = this.Zones.ZoneList[skey].SchoolList.Element;
            this.updateGUI(this.Zones.ZoneList[skey]);
            var succeed = true;
            var dateStart = school.DateStart.replace(':', '-');
            var dateEnd = school.DateEnd.replace(':', '-');
            this.send(">Create:school:" + zoneID + ":" + school.ID + ":" + school.Name + ":" + dateStart + ":" + dateEnd + ":;");
        }
    }
    if (succeed) {
        var wsh = this;
        $("tr .remove", schoolsTable).on('click', function () {
            wsh.removeSchool($(this).parents("tr").attr("id"));
        });
    }
}

WebSocketHandler.prototype.removeSchool = function (schoolId) {
    for (var zkey in this.Zones.ZoneList) {
        if (this.Zones.ZoneList[zkey].SchoolList.Remove(schoolId)) {
            this.updateGUI(this.Zones.ZoneList[zkey]);
            console.log("remove school: " + schoolId);
            break;
        } else {
            console.log("could not be deleted");
            return false;
        }
    }
    this.send(">Remove:school:" + schoolId + ":;");
}

WebSocketHandler.prototype.newRoadConstruction = function (roadConstruction, zoneID) {

    for (var skey in this.Zones.ZoneList) {
        if (this.Zones.ZoneList[skey].ID == zoneID) {
            this.Zones.ZoneList[skey].RoadConstructionList.Add(roadConstruction);
            var roadConstructionTable = this.Zones.ZoneList[skey].RoadConstructionList.Element;
            this.updateGUI(this.Zones.ZoneList[skey]);
            var succeed = true;
            this.send(">Create:roadconstruction:" + zoneID + ":" + roadConstruction.ID + ":" + roadConstruction.Name + ":" + roadConstruction.DateStart + ":" + roadConstruction.DateEnd + ":;");
        }
    }
    if (succeed) {
        var wsh = this;
        $("tr .remove", roadConstructionTable).on('click', function () {
            wsh.removeRoadConstruction($(this).parents("tr").attr("id"));
        });
    }
}

WebSocketHandler.prototype.removeRoadConstruction = function (roadConstructionId) {
    for (var zkey in this.Zones.ZoneList) {
        if (this.Zones.ZoneList[zkey].RoadConstructionList.Remove(roadConstructionId)) {
            this.updateGUI(this.ZoneList.ZoneList[zkey]);

            console.log("remove roadConstruction: " + roadConstructionId);
            break;
        } else {
            console.log("could not be deleted");
            return false;
        }
    }
    this.send(">remove:roadconstruction:" + roadConstructionId + ":;");
}

WebSocketHandler.prototype.updateGUI = function (zone) {
    this.Zones.updateGUI(zone);
    //Fix so the Zone Remove event keeps working... refactor into method
    var wsh = this;
    $("tr .remove", this.Zones.Element).on('click', function () {
        wsh.removeZone($(this).parents("tr").attr("id"));
    });
}

WebSocketHandler.prototype.removeAll = function () {
    console.log(this.Zones.ZoneList.length);
    zonelist = this.Zones.ZoneList;
    for (var zkey in zonelist) {
        var zone = zonelist[zkey];
        for (var skey in zone.SchoolList.SchoolList) {
            zone.SchoolList.Remove(zone.SchoolList.SchoolList[skey].ID);
        }
        for (var rkey in zone.RoadConstructionList.RoadConstructionList) {
            zone.RoadConstructionList.Remove(zone.RoadConstructionList.RoadConstructionList[rkey].ID);
        }
    }
    this.Zones.RemoveAll();
}

WebSocketHandler.prototype.getData = function () {
    //Ask for all Zones
    console.log('Get All Zones');
    this.send(">GET:zones:;");

    console.log('Get Com Ports');
    this.send(">GET:Ports:;");

    //Ask for all schools,roadConstructions,Vertexes en Edges
    for (var key in this.Zones.ZoneList) {
        console.log('Get Schools for Zone:' + this.Zones.ZoneList[key].ID);
        this.send(">GET:schools:" + this.Zones.ZoneList[key].ID + ":;");

        console.log('Get RoadConstructions for Zone:' + this.Zones.ZoneList[key].ID);
        this.send(">GET:roadconstructions:" + this.Zones.ZoneList[key].ID + ":;");

        console.log('Get Vertexes for Zone:' + this.Zones.ZoneList[key].ID);
        this.send(">GET:vertex:" + this.Zones.ZoneList[key].ID + ":;");

        console.log('Get Edges for Zone:' + this.Zones.ZoneList[key].ID);
        this.send(">GET:edges:" + this.Zones.ZoneList[key].ID + ":;");
    };


};