function View(name, viewElement) {
    this.Element = viewElement;
    this.Name = name;

    this.Show = function () {
        this.Element.show();
        dl(this);
    };
    this.Hide = function () {
        this.Element.hide();
    };
}

function Views() {
    this.ViewsList = [];

    this.Add = function (name, viewElement) {
        var newView = new View(name, viewElement);
        this.ViewsList.push(newView);
        dl(this.ViewsList);
    };
    this.Show = function (name) {
        this.hideAll();
        this.ViewsList.some(function (view) {
            if (view.Name == name) {
                view.Show();
                return true;
            }
        });
    };
    this.hideAll = function () {
        for (var key in this.ViewsList) {
            this.ViewsList[key].Hide();
        }
    };
}