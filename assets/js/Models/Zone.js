function Zone(id, name) {
    this.Name = name;
    this.ID = id;
    this.X;
    this.Y;
    this.SchoolList = new Schools($("#schoolTable"));
    this.RoadConstructionList = new RoadConstructions($("#roadConstructionTable"));
    this.VertexList = [];
    this.EdgeList = [];
}

function Zones(table) {
    this.ZoneList = new Array();
    this.Element = table;
}

Zones.prototype.Add = function (newZone) {
    this.ZoneList.push(newZone);
    console.log(this.ZoneList);
    this.addToGUI(newZone);
}
Zones.prototype.addToGUI = function (newZone) {
    $("> tbody", this.Element).append('<tr id="' + newZone.ID + '"><td>' + newZone.ID + '</td><td>' + newZone.Name + '</td><td> ' + newZone.SchoolList.SchoolList.length + ' </td><td>' + newZone.RoadConstructionList.RoadConstructionList.length + '</td><td>' + newZone.VertexList.length + '</td><td>' + newZone.EdgeList.length + '</td><td><span class="remove glyphicon glyphicon-remove" aria-hidden="true"></span></td></tr>');
}
Zones.prototype.updateGUI = function (zone) {
    $("> tbody > tr#" + zone.ID, this.Element).html('<td>' + zone.ID + '</td><td>' + zone.Name + '</td><td> ' + zone.SchoolList.SchoolList.length + ' </td><td>' + zone.RoadConstructionList.RoadConstructionList.length + '</td><td>' + zone.VertexList.length + '</td><td>' + zone.EdgeList.length + '</td><td><span class="remove glyphicon glyphicon-remove" aria-hidden="true"></span></td>');
}
Zones.prototype.Remove = function (id) {
    for (var key in this.ZoneList) {
        if (this.ZoneList[key].ID == id) {
            this.removeFromGUI(this.ZoneList[key]);
            this.ZoneList.splice(key, 1);
            return true;
        }
    }
    return false;
}
Zones.prototype.removeFromGUI = function (Zone) {
    $('#' + Zone.ID, this.Element).remove();
}