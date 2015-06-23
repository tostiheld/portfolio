var parser = function (message, self) {
    var json;
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
                startVertexId: json.startVertexId,
                radarVertexId: json.radarVertexId,
                arduinoPort: json.arduinoPort
            };
            self.Zones.addZone(newZone);
            break;
        case "createschool":
            var newSchool = {
                zoneID: json.zoneId,
                Name: json.name,
                VertexId: json.location,
                DateStart: json.dateStart,
                DateEnd: json.dateEnd
            };
            self.Zones.addSchool(newSchool);
            break;
        case "createroadconstruction":
            var newRoadConstruction = {
                zoneID: json.zoneId,
                Name: json.name,
                VertexId: json.location,
                DateStart: json.dateStart,
                DateEnd: json.dateEnd
            };
            self.Zones.addRoadConstruction(newRoadConstruction);
            break;
        case "createvertex":
            var newVertex = {
                zoneID: json.zoneId,
                Name: json.name,
                DateStart: json.dateStart,
                DateEnd: json.dateEnd
            };
            self.Zones.addVertex(newVertex);
            break;
        case "createedgeset":
//            var newVertex = {
//                zoneID: json.zoneId,
//                Name: json.name,
//                DateStart: json.dateStart,
//                DateEnd: json.dateEnd
//            };
//            self.Zones.addRoadConstruction(newRoadConstruction);
//            break;
        case "removezone":
            break;
        case "removeschool":
            break;
        case "removeroadconstruction":
            break;
        case "removevertex":
            break;
        case "removeedge":
            break;
        case "requestzones":
            json.requestedObjects.forEach(function(element, index){
                var newZone = {
                    id: element.ZoneId,
                    name: element.Name,
                    startVertexId: element.StartVertexId,
                    radarVertexId: element.RadarVertexId,
                    arduinoPort: element.ArduinoPort
                };
                console.log(element);
                self.Zones.addZone(newZone);
            });
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