function WebSocketHandler(console) {
    this.con = console;
    this.WebSocketC = null;
    //create some data
    this.Zones = new ZoneModel([new zone(new school(), new roadc()), new zone(new school(), new roadc())]);
    ko.applyBindings(this.Zones);


    this.send = function (message) {
        this.con.appendFromClient(message);
        this.WebSocketC.ws.send(message);
    };
    this.onOpen = function () {
        //Hide connected and show disconnect button
        $(".connect").hide();
        $(".disconnect").show();
        //Enable send message
        $("#send").removeClass("disabled");
        // Send identification, so the server knows we are an UI
        this.send(">IDEN:UI:;");

        //create some more data
        this.Zones.addZone(1, "testManual");
        this.Zones.addZone(2, "test2Manual");

        //ask for existing data
        //this.getData();
    };
    this.onMessage = function (e) {
        var message = e.data;
        this.con.appendFromServer(message);
        var json;
        try {
            json = JSON.parse(message);
        } catch (ev) {
            console.log("this is no json");
        }
        //if ports
        var self = this;
        if (json.payloadtype == "ports") {
            self.Zones.availableArduinoPorts.removeAll();
            $.each(json.payload, function (key) {
                self.Zones.availableArduinoPorts.push(this);
            });
        } else if (json.payloadtype == "temperature") {
            var minT = -30;
            var maxT = 50;
            var tempT = json.payload[0] + -(minT);
            var percentT = tempT / (maxT + -(minT)) * 100;
            $(".temp").css("width", percenT + "%");
        }
        dl("message type:" + json.payloadtype + ", Message: " + json.payload);
    };
    this.onClose = function () {
        $(".connect").show();
        $(".disconnect").hide();
        $("#send").addClass("disabled");
        this.Zones.removeAll();
    };
    this.onError = function (e) {
        var message = e.data;
        this.con.appendError(message);
    };
    this.onDisconnect = function () {
        this.send(">DISC:;");
        this.Zones.removeAll();
    };
    this.addZone = function (newZone) {
        var x = "0";
        if (typeof newZone.X != "undefined") {
            x = newZone.X;
        }
        var y = "0";
        if (typeof newZone.Y != "undefined") {
            y = newZone.Y;
        }
        this.send(">Create:zone:" + newZone.ID + ":" + x + ":" + y + ":;");
    };
    this.removeZone = function (id) {
        this.send(">Remove:zone:" + id + ":;");
    };

    this.addSchool = function (zoneID, school) {
        var dateStart = school.DateStart.replace(':', '-');
        var dateEnd = school.DateEnd.replace(':', '-');
        this.send(">Create:school:" + zoneID + ":" + school.ID + ":" + school.Name + ":" + dateStart + ":" + dateEnd + ":;");
    };
    this.removeSchool = function (schoolId) {
        this.send(">Remove:school:" + schoolId + ":;");
    };
    this.addRoadC = function (zoneID, roadc) {
        this.send(">Create:roadconstruction:" + zoneID + ":" + roadc.ID + ":" + roadc.Name + ":" + roadc.DateStart + ":" + roadc.DateEnd + ":;");
    };
    this.removeRoadC = function (roadConstructionId) {
        this.send(">remove:roadconstruction:" + roadConstructionId + ":;");
    };
    this.connectArduino = function (zoneID, portName) {
        this.send(">SET:zone:" + zoneID + ":road:" + portName + ":;");
    };
    this.getData = function () {
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
        }


    };
}