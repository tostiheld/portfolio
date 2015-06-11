var ZoneModel = function (zones) {
    var self = this;

    //
    // LOAD ZONES ARRAY FROM CONSTRUCTOR TO OBSERVABLE ARRAY
    //
    self.zones = ko.observableArray(ko.utils.arrayMap(zones, function (zone) {
        return {
            ID: zone.ID,
            Name: ko.observable(zone.Name),
            Arduino: ko.observable(zone.Arduino),
            Schools: ko.observableArray(ko.utils.arrayMap(zone.Schools, function (school) {
                return {
                    zoneID: school.zoneID,
                    Name: school.Name,
                    DateStart: school.DateStart,
                    DateEnd: school.DateEnd
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
            Edges: ko.observableArray(zone.Edges)
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
            DateEnd: $("input[name='dateEnd']", formElement).val()
        };

        //send School to Server
        window.Handler.addSchool(newSchool.zoneID, newSchool);

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
            zoneID: $("select[name='zoneID']", formElement).val()
            Name: $("input[name='roadcName']", formElement).val(),
            DateStart: $("input[name='dateStart']", formElement).val(),
            DateEnd: $("input[name='dateEnd']", formElement).val()
        };

        //send to server
        window.Handler.addRoadC(zoneID, newRoadC);

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
        }

        // Send to server
        window.Handler.addZone(newZone);

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
            Arduino: ko.observable("")
        };

        // Add to zones list
        self.zones.push(newZoneKO);

    };
    
    //
    // ADD EDGE
    //
    self.addEdge = function (newEdge) {

        //find zone
        var zone = self.findZoneByID(newEdge.zoneID);

        //add school to zone
        zone.Edges.push(newEdge);

    };
    
        
    //
    // ADD VERTEX
    //
    self.addVertex = function (newVertex) {

        //find zone
        var zone = self.findZoneByID(newVertex.zoneID);

        //add school to zone
        zone.Vertexes.push(newVertex);

    };


    //
    // ADD ZONE FROM GUI
    //
    self.UIaddArduino = function (formElement) {
        // Send to server
        window.Handler.connectArduino($("input[name='id']", formElement).val(), $("select[name='arduinoPort']", formElement).val());

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
                console.log(zone);
            }
        });
        return zone;
    };



    //
    // REMOVE ZONE FROM UI
    //
    self.UIremoveZone = function (zone) {
        //send to server
        window.Handler.removeZone(zone.ID);
    };

    self.removeZone = function (zone) {
        self.zones.remove(zone);
    }

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
        window.Handler.removeSchool(school.ID);
    };

    //
    // REMOVE SCHOOL FROM UI
    //
    self.removeSchool = function (school) {
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
        window.Handler.removeZone(roadc.ID);
    };

    //
    // REMOVE ROADCONSTRUCTION FROM UI
    //
    self.removeRoadC = function (roadc) {
        //send to server
        window.Handler.removeZone(roadc.ID);
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
};