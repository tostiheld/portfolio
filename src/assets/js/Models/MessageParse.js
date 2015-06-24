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
                name: json.Name,
                startVertexId: json.StartVertexId,
                radarVertexId: json.RadarVertexId,
                arduinoPort: json.ArduinoPort
            };
            self.Zones.addZone(newZone);
            break;
        case "createschool":
            var newSchool = {
                schoolId: SchoolId,
                zoneID: json.ZoneId,
                Name: json.name,
                VertexId: json.VertexId,
                DateStart: json.OpenTime,
                DateEnd: json.CloseTime
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
            self.Zones.addRoadC(newRoadConstruction);
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
            var newVertex2 = json.startVertex;
            self.Zones.addVertex(newVertex2);
            
            var newVertex3 = json.endVertex;
            self.Zones.addVertex(newVertex3);
            
            var newEdge = json.createdEdge;
            newEdge.startX = newVertex2.X;
            newEdge.startY = newVertex2.Y;
            newEdge.endX = newVertex3.X;
            newEdge.endY = newVertex3.Y;
            self.Zones.addEdge(newEdge);
            window.AN.redrawLines();
            
            break;
        case "removezone":
            self.Zones.removeZone(json.removedObject);
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