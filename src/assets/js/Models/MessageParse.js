var parser = function (json, self) {
    try {
        json = JSON.parse(message);
    } catch (ev) {
        console.log("this is no json");
    }

    switch(json.command.toLowerCase()) {
        case "createzone":
            var newZone = {
                id: json.zoneId,
                name: json.name,
                startVertex: json.startVertex,
                radarVertex: json.radarVertex,
                arduinoPort: json.arduinoPort
            };
            self.Zones.addZone(newZone);
            break;
        case "createschool":
            var newSchool = {
                zoneID: json.zoneId,
                Name: json.name,
                DateStart: json.dateStart,
                DateEnd: json.dateEnd
            };
            self.Zones.addSchool(newSchool);
            break;
        case "createroadconstruction":
            var newRoadConstruction = {
                zoneID: json.zoneId,
                Name: json.name,
                DateStart: json.dateStart,
                DateEnd: json.dateEnd
            };
            self.Zones.addRoadConstruction(newRoadConstruction);
            break;
        case "createVertex":
            var newVertex = {
                zoneID: json.zoneId,
                Name: json.name,
                DateStart: json.dateStart,
                DateEnd: json.dateEnd
            };
            self.Zones.addRoadConstruction(newRoadConstruction);
            break;
    }
};




/*
    if (json.payload.ports != "undefined") {
        self.Zones.availableArduinoPorts.removeAll();
        $.each(json.payload, function (key) {
            self.Zones.availableArduinoPorts.push(this);
        });
    } else if (json.payload.temperature != "undefined") {
        var minT = -30;
        var maxT = 50;
        var tempT = json.payload[0] + -(minT);
        var percentT = tempT / (maxT + -(minT)) * 100;
        $(".temp").css("width", percenT + "%");
    }*/