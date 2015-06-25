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
                id: json.createdObject.zoneId,
                name: json.createdObject.Name,
                startVertexId: json.createdObject.StartVertexId,
                RadarVertex: json.createdObject.RadarVertexId,
                arduinoPort: json.createdObject.ArduinoPort
            };
            self.Zones.addZone(newZone);
            break;
        case "createschool":
            var newSchool = {
                ID: json.createdObject.SchoolId,
                zoneID: json.createdObject.ZoneId,
                Name: json.createdObject.Name,
                VertexId: json.createdObject.VertexId,
                DateStart: json.createdObject.OpenTime,
                DateEnd: json.createdObject.CloseTime
            };
            self.Zones.addSchool(newSchool);
            console.log(newSchool);
            break;
        case "createroadconstruction":
            var newRoadConstruction = {
                ID: json.createdObject.RoadConstructionId,
                zoneID: json.createdObject.ZoneId,
                Name: json.createdObject.Name,
                VertexId: json.createdObject.VertexId,
                DateStart: json.createdObject.DateStart,
                DateEnd: json.createdObject.DateEnd
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
            self.Zones.removeSchool(json.removedObject);
            break;        
        case "removeroadconstruction":
            self.Zones.removeRoadC(json.removedObject);
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
                    RadarVertex: element.RadarVertexId,
                    arduinoPort: element.ArduinoPort
                };
                self.Zones.addZone(newZone);
            });
            break;
        case "requestschools":
            json.requestedObjects.forEach(function(element, index){
                var newSchool = {
                    ID: element.SchoolId,
                    zoneID: element.ZoneId,
                    Name: element.Name,
                    VertexId: element.VertexId,
                    DateStart: element.OpenTime,
                    DateEnd: element.CloseTime
                };
                self.Zones.addSchool(newSchool);
            });
            break;
        case "requestroadconstructions":
            json.requestedObjects.forEach(function(element, index){
                var newRoadConstruction = {
                    ID: element.RoadConstructionId,
                    zoneID: element.ZoneId,
                    Name: element.Name,
                    EdgeId: element.EdgeId,
                    DateStart: element.DateStart,
                    DateEnd: element.DateEnd
                };
                self.Zones.addRoadC(newRoadConstruction);
            });
            break;
        case "requestmap":
            json.vertices.forEach(function(element, index){
                self.Zones.addVertex(element);
            });
            
            json.edges.forEach(function(element, index){
                var startVertex = self.Zones.findVertexByID(element.StartVertexId);
                element.startX = startVertex.X;
                element.startY = startVertex.Y;
                var endVertex = self.Zones.findVertexByID(element.EndVertexId);
                element.endX = endVertex.X;
                element.endY = endVertex.Y;
                
                
                self.Zones.addEdge(element);
            });
            break;
        case "getroads":
            self.Zones.availableArduinoPorts.removeAll();
            json.ports.forEach(function(element, index){
                self.Zones.availableArduinoPorts.push(element);
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