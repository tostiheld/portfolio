function Zone(id, name, x, y) {
    this.Name = name;
    this.ID = id;
    this.X = x;
    this.Y = y;
    this.SchoolList = new Schools();
    this.RoadConstructionList = new RoadConstructions();
    this.VertexList = [];
    this.EdgeList = [];
}

function Zones(table) {
    this.ZoneList = [];
    this.Element = table;
}

Zones.prototype.Add = function (newZone) {
    this.ZoneList.push(newZone);
    this.addToGUI(newZone);
}
Zones.prototype.addToGUI = function (newZone) {
    $("> tbody", this.Element).append('<tr id="' + newZone.ID + '"><td>' + newZone.ID + '</td><td>' + newZone.Name + '</td><td> ' + newZone.SchoolList.length + ' </td><td>' + newZone.RoadConstructionList.length + '</td><td>' + newZone.VertexList.length + '</td><td>' + newZone.EdgeList.length + '</td><td><span class="remove glyphicon glyphicon-remove" aria-hidden="true"></span></td></tr>');
}
Zones.prototype.Remove = function (id) {
    for (var key in this.ZoneList) {
        if (this.ZoneList[key].ID == id) {
            this.removeFromGUI(this.ZoneList[key]);
            this.ZoneList.splice(key, 1);
            console.log(this.ZoneList);
            break;
        }
    };
}
Zones.prototype.removeFromGUI = function (Zone) {
    $('#' + Zone.ID, this.Element).remove();
}