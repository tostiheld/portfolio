function WebSocketHandler(console) {
    this.con = console;
    this.WebSocketC;
    this.Zones = new ZoneModel([new zone(new school(), new roadc()), new zone(new school, new roadc())]);
    ko.applyBindings(this.Zones);
}
WebSocketHandler.prototype.send = function (message) {
    this.con.appendFromClient(message);
    this.WebSocketC.ws.send(message);
}
WebSocketHandler.prototype.onOpen = function () {
    $(".connect").hide();
    $(".disconnect").show();
    $("#send").removeClass("disabled");
    this.send(">IDEN:UI:;");

    this.Zones.addZone(1, "testManual");
    this.Zones.addZone(2, "test2Manual");
    //this.getData();
};
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
    } else if (json['payloadtype'] == "temperature") {
        var minT = -30;
        var maxT = 50;
        var tempT = json['payload'][0] + -(minT);
        var percentT = tempT / (maxT + -(minT)) * 100;
        $(".temp").css("width", percenT + "%");
    }
    dl("message type:" + json['payloadtype'] + ", Message: " + json['payload']);
};
WebSocketHandler.prototype.onClose = function () {
    $(".connect").show();
    $(".disconnect").hide();
    $("#send").addClass("disabled");
};
WebSocketHandler.prototype.onError = function (e) {
    var message = e.data;
    this.con.appendError(message);
};
WebSocketHandler.prototype.onDisconnect = function () {
    this.send(">DISC:;");
    this.Zones.removeAll();
}
WebSocketHandler.prototype.addZone = function (newZone) {
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
}
WebSocketHandler.prototype.removeZone = function (id) {
    this.send(">Remove:zone:" + id + ":;");
}

WebSocketHandler.prototype.addSchool = function (zoneID, school) {
    var dateStart = school.DateStart.replace(':', '-');
    var dateEnd = school.DateEnd.replace(':', '-');
    this.send(">Create:school:" + zoneID + ":" + school.ID + ":" + school.Name + ":" + dateStart + ":" + dateEnd + ":;");
}
WebSocketHandler.prototype.removeSchool = function (schoolId) {
    this.send(">Remove:school:" + schoolId + ":;");
}
WebSocketHandler.prototype.addRoadC = function (zoneID, roadc) {
    this.send(">Create:roadconstruction:" + zoneID + ":" + roadc.ID + ":" + roadc.Name + ":" + roadc.DateStart + ":" + roadc.DateEnd + ":;");
}
WebSocketHandler.prototype.removeRoadC = function (roadConstructionId) {
    this.send(">remove:roadconstruction:" + roadConstructionId + ":;");
}
WebSocketHandler.prototype.getData = function () {
    //Ask for all Zones
    dl('Get All Zones');
    this.send(">GET:zones:;");

    dl('Get Com Ports');
    this.send(">GET:Ports:;");

    //Ask for all schools,roadConstructions,Vertexes en Edges
    for (var key in this.Zones.ZoneList) {
        dl('Get Schools for Zone:' + this.Zones.ZoneList[key].ID);
        this.send(">GET:schools:" + this.Zones.ZoneList[key].ID + ":;");

        dl('Get RoadConstructions for Zone:' + this.Zones.ZoneList[key].ID);
        this.send(">GET:roadconstructions:" + this.Zones.ZoneList[key].ID + ":;");

        dl('Get Vertexes for Zone:' + this.Zones.ZoneList[key].ID);
        this.send(">GET:vertex:" + this.Zones.ZoneList[key].ID + ":;");

        dl('Get Edges for Zone:' + this.Zones.ZoneList[key].ID);
        this.send(">GET:edges:" + this.Zones.ZoneList[key].ID + ":;");
    };


};