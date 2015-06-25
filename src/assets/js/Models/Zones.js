var ZoneModel = function (zones) {
    var self = this;
    zones = null;
    //
    // LOAD ZONES ARRAY FROM CONSTRUCTOR TO OBSERVABLE ARRAY
    //
    self.zones = ko.observableArray(ko.utils.arrayMap(zones, function (zone) {
        return {
            ID: zone.ID,
            Name: ko.observable(zone.Name),
            Arduino: ko.observable(zone.arduinoPort),
            Schools: ko.observableArray(ko.utils.arrayMap(zone.Schools, function (school) {
                return {
                    ID: school.ID,
                    zoneID: school.zoneID,
                    Name: school.Name,
                    DateStart: school.DateStart,
                    DateEnd: school.DateEnd,
                    VertexId: school.VertexId,
                    DateStart: school.OpenTime,
                    DateEnd: school.CloseTime
                };
            })),
            RoadConstructions: ko.observableArray(ko.utils.arrayMap(zone.RoadConstructions, function (roadc) {
                return {
                    ID: roadc.ID,
                    Name: roadc.Name,
                    DateStart: roadc.DateStart,
                    DateEnd: roadc.DateEnd
                };
            })),
            Vertexes: ko.observableArray(zone.Vertexes),
            Edges: ko.observableArray(zone.Edges),
            StartVertex: ko.observable(zone.startVertex),
            RadarVertex: ko.observable(zone.radarVertex),
        };
    }));

    //
    // MAKE AN OBSERVABLE ARDUINO PORTS LIST
    //
    self.availableArduinoPorts = ko.observableArray();

    //
    // ADD SCHOOL FROM GUI
    //
    self.UIaddSchool = function (formElement) {

        var newSchool = {
            zoneID: $("select[name='zoneID']", formElement).val(),
            Name: $("input[name='schoolName']", formElement).val(),
            DateStart: $("input[name='dateStart']", formElement).val(),
            DateEnd: $("input[name='dateEnd']", formElement).val(),
            location: $("input[name='schoolVertexId']", formElement).val(),
        };

        //send School to Server
        window.Handler.MessageStringify.addSchool(newSchool.zoneID, newSchool);

        //empty form
        clearForm(formElement);

        //close popup
        $(".modal.in").modal('hide');
    };

    //
    // ADD SCHOOL
    //
    self.addSchool = function (newSchool) {
        //find zone
        var zone = self.findZoneByID(newSchool.zoneID);

        //add school to zone
        zone.Schools.push(newSchool);

    };

    //
    // ADD ROADCONSTRUCTION FROM GUI
    //
    self.UIaddRoadC = function (formElement) {


        var newRoadC = {
            zoneID: $("select[name='zoneID']", formElement).val(),
            Name: $("input[name='roadcName']", formElement).val(),
            DateStart: $("input[name='dateStart']", formElement).val(),
            DateEnd: $("input[name='dateEnd']", formElement).val(),
            location: $("input[name='roadConstructionEdgeId']", formElement).val()
        };

        //send to server
        window.Handler.MessageStringify.addRoadC(newRoadC.zoneID, newRoadC);

        //empty form
        clearForm(formElement);

        //close popup
        $(".modal.in").modal('hide');
    };

    //
    // ADD School
    //
    self.addRoadC = function (newRoadC) {
        //find zone
        var zone = self.findZoneByID(newRoadC.zoneID);

        //add school to zone
        zone.RoadConstructions.push(newRoadC);
    };

    //
    // ADD ZONE FROM GUI
    //
    self.UIaddZone = function (formElement) {

        var newZone = {
            name: $("input[name='zoneName']", formElement).val()
        };

        // Send to server
        window.Handler.MessageStringify.addZone(newZone);

        //empty form
        clearForm(formElement);

        //close popup
        $(".modal.in").modal('hide');
    };

    //
    // ADD ZONE
    //
    self.addZone = function (newZone) {

        //Needed for KO
        var newZoneKO = {
            ID: newZone.id,
            Name: ko.observable(newZone.name),
            Schools: ko.observableArray(),
            RoadConstructions: ko.observableArray(),
            Vertexes: ko.observableArray(),
            Edges: ko.observableArray(),
            StartVertex: ko.observable(newZone.startVertexId),
            RadarVertex: ko.observable(newZone.RadarVertex),
            Arduino: ko.observable(newZone.arduinoPort)
        };

        // Add to zones list
        self.zones.push(newZoneKO);

    };
    
    //
    // ADD EDGE FROM GUI
    //
    self.UIaddEdge = function (data) {

        console.log(data);
        var newEdgeSet = {
            startVertexX: data[0],
            startVertexY: data[1],
            endVertexX: data[2],
            endVertexY: data[3]
        };

        //send edgeSet to Server
        window.Handler.MessageStringify.addEdgeSet($(".canvas_zoneID").val(), newEdgeSet);
    };
    
    //
    // ADD EDGE
    //
    self.addEdge = function (newEdge) {

        //find zone
        var zone = self.findZoneByID(newEdge.ZoneId);

        //add school to zone
        zone.Edges.push(newEdge);

    };
    
        
    //
    // ADD VERTEX
    //
    self.addVertex = function (newVertex) {

        //find zone
        var zone = self.findZoneByID(newVertex.ZoneId);

        //add school to zone
        zone.Vertexes.push(newVertex);

    };


    //
    // ADD ZONE FROM GUI
    //
    self.UIaddArduino = function (formElement) {
        // Send to server
        window.Handler.MessageStringify.connectArduino($("input[name='id']", formElement).val(), $("select[name='arduinoPort']", formElement).val(),$("input[name='portVertexId']", formElement).val());
        
        var zone = self.findZoneByID($("input[name='id']", formElement).val());
        var index = self.zones.indexOf(zone);
        self.zones()[index].RadarVertex($("input[name='portVertexId']", formElement).val());
        self.zones()[index].Arduino($("select[name='arduinoPort']", formElement).val());
        
        
        //empty form
        clearForm(formElement);
        //close popup
        $(".modal.in").modal('hide');
    };

    //
    // ADD Arduino
    //
    self.addArduino = function (zoneID, arduinoPort) {
        // find correct zone

        var zone = self.findZoneByID(zoneID);
        var index = self.zones.indexOf(zone);
        // Add arduino to zone
        console.log("index:" + index);
        console.log(zone);
        self.zones()[index].Arduino(arduinoPort);

        //self.zones.push(self.zones()[index]);

    };

    //
    // FIND ZONE BY ID
    //
    self.findZoneByID = function (zoneID) {
        var zone;
        ko.utils.arrayFirst(self.zones(), function (tzone) {
            if (tzone.ID == zoneID) {
                zone = tzone;
            }
        });
        return zone;
    };
    
    
    //
    // FIND School BY ID
    //
    self.findSchoolByID = function (schoolID) {
        var school;
        ko.utils.arrayFirst(self.zones(), function (tzone) {
            ko.utils.arrayFirst(tzone.Schools(), function (tschool) {
                if(tschool.ID == schoolID){
                    school = tschool;
                }
            });
        });
        return school;
    };

    //
    // FIND Vertex BY ID
    //
    self.findVertexByID = function (vertexID) {
        var vertex;
        ko.utils.arrayFirst(self.zones(), function (tzone) {
            ko.utils.arrayFirst(tzone.Vertexes(), function (tvertex) {
                if(tvertex.VertexId == vertexID){
                    vertex = tvertex;
                }
            });
        });
        return vertex;
    };

    //
    // FIND Edge BY ID
    //
    self.findEdgeByID = function (edgeID) {
        var edge;
        ko.utils.arrayFirst(self.zones(), function (tzone) {
            ko.utils.arrayFirst(tzone.Edges(), function (tedge) {
                if(tedge.EdgeId == edgeID){
                    edge = tedge;
                }
            });
        });
        return edge;
    };

    //
    // REMOVE ZONE FROM UI
    //
    self.UIremoveZone = function (zone) {
        //send to server
        window.Handler.MessageStringify.removeZone(zone.ID);
    };

    self.removeZone = function (zoneId) {
        var zone = self.findZoneByID(zoneId);
        self.zones.remove(zone);
    };

    //
    // REMOVE ALL ZONES
    // 
    self.removeAll = function (zone) {
        self.zones.removeAll();
        //don't send to server because this is just for disconnects
    };

    //
    // REMOVE SCHOOL FROM UI
    //
    self.UIremoveSchool = function (school) {
        //send to server
        window.Handler.MessageStringify.removeSchool(school.ID);
    };

    //
    // REMOVE SCHOOL FROM UI
    //
    self.removeSchool = function (schoolId) {
        var school = self.findSchoolByID(schoolId);
        console.log(school);
        $.each(self.zones(), function () {
            this.Schools.remove(school);
        });
    };

    //
    // REMOVE ROADCONSTRUCTION FROM UI
    //
    self.UIremoveRoadC = function (roadc) {
        $.each(self.zones(), function () {
            this.RoadConstructions.remove(roadc);
        });

        //send to server
        window.Handler.MessageStringify.removeRoadC(roadc.ID);
    };

    //
    // REMOVE ROADCONSTRUCTION FROM UI
    //
    self.removeRoadC = function (roadConstructionId) {
        var roadConstruction = self.findRoadConstructionByID(roadConstructionId);
        console.log(roadConstruction);
        $.each(self.zones(), function () {
            this.Schools.remove(roadConstruction);
        });
    };
};



/////////
///////// JUST SOME GAY TEST DATA
/////////

var school = function () {
    this.ID = "5";
    this.Name = "Doe";
    this.DateStart = "";
    this.DateEnd = "";
};

var roadc = function () {
    this.ID = "78";
    this.Name = "roadconstruction";
    this.DateStart = "23232";
    this.DateEnd = "24778";
};

var zone = function (school, roadc) {
    this.ID = 1;
    this.Name = "Test";
    this.Schools = [school, school];
    this.RoadConstructions = [roadc, roadc];
    this.Vertexes = [];
    this.Edges = [];
    this.Arduino = "";
    this.StartVertex = 1;
    this.RadarVertex = 1;
    this.Arduino = "";
};