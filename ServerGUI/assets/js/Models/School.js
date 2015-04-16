function School(name, dateStart, dateEnd, id) {
    this.Name = name;
    this.DateStart = dateStart;
    this.DateEnd = dateEnd;
    this.ID = id;
}

function Schools(table) {
    this.SchoolList = [];
    this.Element = table;
}

Schools.prototype.Add = function (newSchool) {
    this.SchoolList.push(newSchool);
    this.addToGUI(newSchool);
}
Schools.prototype.addToGUI = function (newSchool) {
    $("> tbody", this.Element).append('<tr id="' + newSchool.ID + '"><td>' + newSchool.ID + '</td><td>' + newSchool.Name + '</td><td> ' + newSchool.DateStart + ' </td><td>' + newSchool.DateEnd + '</td><td><span class="remove glyphicon glyphicon-remove" aria-hidden="true"></span></td></tr>');
}
Schools.prototype.Remove = function (id) {
    for (var key in this.SchoolList) {
        if (this.SchoolList[key].ID == id) {
            this.removeFromGUI(this.SchoolList[key]);
            this.SchoolList.splice(key, 1);
            console.log(this.SchoolList);
            return true;
        }
    }
    return false;
}
Schools.prototype.removeFromGUI = function (school) {
    $('#' + school.ID, this.Element).remove();
}