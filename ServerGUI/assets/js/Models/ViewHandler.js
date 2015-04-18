function View(name, viewElement) {
    this.Element = viewElement;
    this.Name = name;
}
View.prototype.Show = function () {
    this.Element.show();
    dl(this);
}
View.prototype.Hide = function () {
    this.Element.hide();
}

function Views() {
    this.ViewsList = []
}
Views.prototype.Add = function (name, viewElement) {
    var newView = new View(name, viewElement);
    this.ViewsList.push(newView);
    dl(this.ViewsList);
}
Views.prototype.Show = function (name) {
    this.hideAll();
    this.ViewsList.some(function (view) {
        if (view.Name == name) {
            view.Show();
            return true;
        }
    });
}

Views.prototype.hideAll = function () {
    for (var key in this.ViewsList) {
        this.ViewsList[key].Hide();
    }
}