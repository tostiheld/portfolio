function WebSocketHandler(console, testMode) {
    this.testMode = testMode | false;
    this.con = console;
    this.WebSocketC = null;
    this.count = 0;
    //create some data
    this.Zones = new ZoneModel([new zone(new school(), new roadc()), new zone(new school(), new roadc())]);
    ko.applyBindings(this.Zones);


    this.send = function (message) {
        this.con.appendFromClient(message);
        this.WebSocketC.ws.send(message, function ack(error) {
            dl(error);
        });
    };
    this.onOpen = function () {
        //Hide connected and show disconnect button
        $(".connect").hide();
        $(".disconnect").show();
        //Enable send message
        $("#send").removeClass("disabled");
        window.AN.initialize(); 

        //ask for existing data
        this.getData();
    };
    this.onMessage = function (e) {
        var message = e;
        this.con.appendFromServer(message);
        parser(message, this);
    };
    this.onClose = function () {
        $(".connect").show();
        $(".disconnect").hide();
        $("#send").addClass("disabled");
        this.Zones.removeAll();
    };
    this.onError = function (e) {
        var message = e;
        this.con.appendError(message);
    };
    this.onDisconnect = function () {
        this.Zones.removeAll();
    };
    this.addZone = function (newZone) {
        //create json
        var json = {};
        this.count++;
        json.id = this.count;
        json.command = "createZone";
        json.name = newZone.name;

        //send json
        this.send(JSON.stringify(json));
    };
    this.removeZone = function (id) {
        //create json
        var json = {};
        this.count++;
        json.id = this.count;
        json.command = "removeZone";
        json.zoneId = id;
        //send json
        this.send(JSON.stringify(json));
    };

    this.addEdgeSet = function (zoneID, newEdgeSet) {
        //create json
        var json = {};
        this.count++;
        json.id = this.count;
        json.command = "createEdgeSet";
        json.zoneId = zoneID;
        json.startVertexX = newEdgeSet.startVertexX;
        json.startVertexY = newEdgeSet.startVertexY;
        json.endVertexX = newEdgeSet.endVertexX;
        json.endVertexY = newEdgeSet.endVertexY;

        //send json
        this.send(JSON.stringify(json));
    };

    this.addSchool = function (zoneID, school) {
        //create json
        var json = {};
        this.count++;
        json.id = this.count;
        json.command = "createSchool";
        json.zoneId = zoneID;
        json.location = 0; //implement vertex id
        json.openTime = school.DateStart;
        json.closeTime = school.DateEnd;

        //send json
        this.send(JSON.stringify(json));
    };
    
    this.removeSchool = function (schoolId) {
        //create json
        var json = {};
        this.count++;
        json.id = this.count;
        json.command = "removeSchool";
        json.schoolId = schoolId;
        //send json
        this.send(JSON.stringify(json));
    };
    
    this.addRoadC = function (zoneID, roadc) {
        //create json
        var json = {};
        this.count++;
        json.id = this.count;
        json.command = "createRoadConstruction";
        json.zoneId = zoneID;
        json.location = 0;
        json.startDate = roadc.DateStart;
        json.endDate = roadc.DateEnd;


        //send json
        this.send(JSON.stringify(json));
    };
    
    this.removeRoadC = function (roadConstructionId) {
        //create json
        var json = {};
        this.count++;
        json.id = this.count;
        json.command = "removeRoadconstruction";
        json.roadconstructionId = roadConstructionId;
        //send json
        this.send(JSON.stringify(json));
    };

    this.connectArduino = function (zoneID, portName) {
        //create json
        var json = {};
        this.count++;
        //        json.id = this.count;
        //        json.command = "removeRoadconstruction";
        //        json.roadconstructionId = ;
        //        json.payload.arduinoPort = portName;
        //send json
        this.send(JSON.stringify(json));
    };

    this.getData = function () {
        //Ask for all Zones
        dl('Get All Zones');
        //create json
        var json = {};
        this.count++;
        json.id = this.count;
        json.command = "requestZones";
        //send json
        this.send(JSON.stringify(json));



//        dl('Get Com Ports');
//        //create json
//        json = {};
//        json.type = 5;
//        json.payload = {};
//        json.payload.roadConstruction = id;
//        //send json
//        this.send(JSON.stringify(json));

    };
}