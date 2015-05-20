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
        //create json
        var json = {};
        json.type = 1;
        json.payload = {};
        json.payload.LinkType = 1;
        //send json
        this.send(JSON.stringify(json));

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
        this.Zones.removeAll();
    };
    this.addZone = function (newZone) {
        var x = 0;
        if (typeof newZone.X != "undefined") {
            x = newZone.X;
        }
        var y = 0;
        if (typeof newZone.Y != "undefined") {
            y = newZone.Y;
        }

        //create json
        var json = {};
        json.type = 4;
        json.payload = {};
        json.payload.Zone = {};
        json.payload.Zone.x = x;
        json.payload.Zone.y = y;
        //send json
        this.send(JSON.stringify(json));
    };
    this.removeZone = function (id) {
        //create json
        var json = {};
        json.type = 5;
        json.payload = {};
        json.payload.Zone = id;
        //send json
        this.send(JSON.stringify(json));
    };

    this.addSchool = function (zoneID, school) {
        var dateStart = school.DateStart.replace(':', '-');
        var dateEnd = school.DateEnd.replace(':', '-');
        //create json
        var json = {};
        json.type = 4;
        json.payload = {};
        json.payload.School = {};
        json.payload.School.Name = school.Name;
        json.payload.School.zoneID = zoneID;
        json.payload.School.dateStart = dateStart;
        json.payload.School.dateEnd = dateEnd;

        //send json
        this.send(JSON.stringify(json));
    };
    this.removeSchool = function (schoolId) {
        //create json
        var json = {};
        json.type = 5;
        json.payload = {};
        json.payload.School = id;
        //send json
        this.send(JSON.stringify(json));
    };
    this.addRoadC = function (zoneID, roadc) {
        //create json
        var json = {};
        json.type = 4;
        json.payload = {};
        json.payload.School = {};
        json.payload.School.Name = school.Name;
        json.payload.School.zoneID = zoneID;
        json.payload.School.dateStart = dateStart;
        json.payload.School.dateEnd = dateEnd;

        //send json
        this.send(JSON.stringify(json));
    };
    this.removeRoadC = function (roadConstructionId) {
        //create json
        var json = {};
        json.type = 5;
        json.payload = {};
        json.payload.RoadConstruction = id;
        //send json
        this.send(JSON.stringify(json));
    };
    this.connectArduino = function (zoneID, portName) {
        //create json
        var json = {};
        json.type = 3;
        json.payload = {};
        json.payload.Zone = id;
        json.payload.arduinoPort = portName;
        //send json
        this.send(JSON.stringify(json));
    };
    this.getData = function () {
        //Ask for all Zones
        dl('Get All Zones');
        //create json
        var json = {};
        json.type = 5;
        json.payload = {};
        json.payload.RoadConstruction = id;
        //send json
        this.send(JSON.stringify(json));



        dl('Get Com Ports');
        //create json
        json = {};
        json.type = 5;
        json.payload = {};
        json.payload.roadConstruction = id;
        //send json
        this.send(JSON.stringify(json));

    };
}