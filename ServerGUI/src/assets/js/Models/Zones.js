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
                    ID: school.ID,
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

        //add School
        self.addSchool($("select[name='zoneID']", formElement).val(),
            $("input[name='schoolID']", formElement).val(),
            $("input[name='schoolName']", formElement).val(),
            $("input[name='dateStart']", formElement).val(),
            $("input[name='dateEnd']", formElement).val());

        //empty form
        clearForm(formElement);

        //close popup
        $(".modal.in").modal('hide');
    };

    //
    // ADD SCHOOL
    //
    self.addSchool = function (zoneID, id, name, dateStart, dateEnd) {
        //find zone
        var zone = self.findZoneByID(zoneID);

        var newSchool = {
            ID: id,
            Name: name,
            DateStart: dateStart,
            DateEnd: dateEnd
        };
        //add school to zone
        zone.Schools.push(newSchool);
        //send to server
        window.Handler.addSchool(zoneID, newSchool);
    };

    //
    // ADD ROADCONSTRUCTION FROM GUI
    //
    self.UIaddRoadC = function (formElement) {

        //add School
        self.addRoadC($("select[name='zoneID']", formElement).val(),
            $("input[name='roadcID']", formElement).val(),
            $("input[name='roadcName']", formElement).val(),
            $("input[name='dateStart']", formElement).val(),
            $("input[name='dateEnd']", formElement).val());

        //empty form
        clearForm(formElement);

        //close popup
        $(".modal.in").modal('hide');
    };

    //
    // ADD School
    //
    self.addRoadC = function (zoneID, id, name, dateStart, dateEnd) {
        //find zone
        var zone = self.findZoneByID(zoneID);
        var newRoadC = {
            ID: id,
            Name: name,
            DateStart: dateStart,
            DateEnd: dateEnd
        };
        //add school to zone
        zone.RoadConstructions.push(newRoadC);
        //send to server
        window.Handler.addRoadC(zoneID, newRoadC);
    };

    //
    // ADD ZONE FROM GUI
    //
    self.UIaddZone = function (formElement) {
        //add zone to zones
        self.addZone($("input[name='zoneID']", formElement).val(), $("input[name='zoneName']", formElement).val());

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
        // Send to server
        window.Handler.connectArduino(zoneID, arduinoPort);
    };


    //
    // ADD ZONE FROM GUI
    //
    self.UIaddArduino = function (formElement) {
        //add zone to zones
        self.addArduino($("input[name='id']", formElement).val(), $("select[name='arduinoPort']", formElement).val());
        //empty form
        clearForm(formElement);
        //close popup
        $(".modal.in").modal('hide');
    };

    //
    // ADD ZONE
    //
    self.addZone = function (id, name) {
        var newZone = {
            ID: id,
            Name: ko.observable(name),
            Schools: ko.observableArray(),
            RoadConstructions: ko.observableArray(),
            Vertexes: ko.observableArray(),
            Edges: ko.observableArray(),
            Arduino: ko.observable("")
        };
        // Add to zones list
        self.zones.push(newZone);
        // Send to server
        window.Handler.addZone(newZone);
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
        self.zones.remove(zone);

        //send to server
        window.Handler.removeZone(zone.ID);
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
        $.each(self.zones(), function () {
            this.Schools.remove(school);
        });

        //send to server
        window.Handler.removeSchool(school.ID);
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