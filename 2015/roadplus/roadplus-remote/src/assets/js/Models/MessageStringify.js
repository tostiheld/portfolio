var MessageStringify = function(self){
    this.addZone = function (newZone) {
        //create json
        var json = {};
        json.command = "createZone";
        json.name = newZone.name;

        //send json
        self.send(JSON.stringify(json));
    };
    this.removeZone = function (id) {
        //create json
        var json = {};
        json.command = "removeZone";
        json.zoneId = id;
        //send json
        self.send(JSON.stringify(json));
    };

    this.addEdgeSet = function (zoneID, newEdgeSet) {
        //create json
        var json = {};
        json.command = "createEdgeSet";
        json.zoneId = zoneID;
        json.startVertexX = newEdgeSet.startVertexX;
        json.startVertexY = newEdgeSet.startVertexY;
        json.endVertexX = newEdgeSet.endVertexX;
        json.endVertexY = newEdgeSet.endVertexY;

        //send json
        self.send(JSON.stringify(json));
    };

    this.addSchool = function (zoneID, school) {
        //create json
        var json = {};
        json.command = "createSchool";
        json.zoneId = zoneID;
        json.name = school.Name;
        json.location = school.location; //implement vertex id
        json.openTime = school.DateStart;
        json.closeTime = school.DateEnd;

        //send json
        self.send(JSON.stringify(json));
    };

    this.removeSchool = function (schoolId) {
        //create json
        var json = {};
        json.command = "removeSchool";
        json.schoolId = schoolId;
        //send json
        self.send(JSON.stringify(json));
    };

    this.addRoadC = function (zoneID, roadc) {
        //create json
        var json = {};
        json.command = "createRoadConstruction";
        json.zoneId = zoneID;
        json.name = roadc.Name;
        json.location = roadc.location;
        json.startDate = roadc.DateStart;
        json.endDate = roadc.DateEnd;


        //send json
        self.send(JSON.stringify(json));
    };

    this.removeRoadC = function (roadConstructionId) {
        //create json
        var json = {};
        json.command = "removeRoadconstruction";
        json.roadconstructionId = roadConstructionId;
        //send json
        self.send(JSON.stringify(json));
    };

    this.connectArduino = function (zoneID, portName, vertexId) {
        //create json
        var json = {};
        json.command = "connectRoad";
        json.zoneId = zoneID;
        json.roadPort = portName;
        json.vertexId = vertexId;
        //send json
        self.send(JSON.stringify(json));
    };

    this.getData = function () {
        //Ask for all Zones
        dl('Get All Zones');
        //create json
        var json = {};
        json.command = "requestZones";
        //send json
        self.send(JSON.stringify(json));

        json.command = "requestSchools";
        //send json
        self.send(JSON.stringify(json));

        json.command = "requestRoadConstructions";
        //send json
        self.send(JSON.stringify(json));

        json.command = "getRoads";
        //send json
        self.send(JSON.stringify(json));

        json.command = "requestMap";
        //send json
        self.send(JSON.stringify(json));

        //        dl('Get Com Ports');
        //        //create json
        //        json = {};
        //        json.type = 5;
        //        json.payload = {};
        //        json.payload.roadConstruction = id;
        //        //send json
        //        this.send(JSON.stringify(json));

    };
};