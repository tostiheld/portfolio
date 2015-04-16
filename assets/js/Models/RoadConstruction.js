function RoadConstruction(name, dateStart, dateEnd, id) {
    this.Name = name;
    this.DateStart = dateStart;
    this.DateEnd = dateEnd;
    this.ID = id;
}

function RoadConstructions(table) {
    this.RoadConstructionList = [];
    this.Element = table;
}

RoadConstructions.prototype.Add = function (newRoadConstruction) {
    this.RoadConstructionList.push(newRoadConstruction);
    this.addToGUI(newRoadConstruction);
}
RoadConstructions.prototype.addToGUI = function (newRoadConstruction) {
    $("> tbody", this.Element).append('<tr id="' + newRoadConstruction.ID + '"><td>' + newRoadConstruction.ID + '</td><td>' + newRoadConstruction.Name + '</td><td> ' + newRoadConstruction.DateStart + ' </td><td>' + newRoadConstruction.DateEnd + '</td><td><span class="remove glyphicon glyphicon-remove" aria-hidden="true"></span></td></tr>');
}
RoadConstructions.prototype.Remove = function (id) {
    for (var key in this.RoadConstructionList) {
        if (this.RoadConstructionList[key].ID == id) {
            this.removeFromGUI(this.RoadConstructionList[key]);
            this.RoadConstructionList.splice(key, 1);
            console.log(this.RoadConstructionList);
            return true;
        }
    }
    return false;
}
RoadConstructions.prototype.removeFromGUI = function (roadConstruction) {
    $('#' + roadConstruction.ID, this.Element).remove();
}