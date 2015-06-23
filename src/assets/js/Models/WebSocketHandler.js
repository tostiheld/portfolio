function WebSocketHandler(console, testMode) {
    this.testMode = testMode | false;
    this.con = console;
    this.WebSocketC = null;
    this.MessageStringify = new MessageStringify(this);
    //create some data
    this.Zones = new ZoneModel([new zone(new school(), new roadc()), new zone(new school(), new roadc())]);
    ko.applyBindings(this.Zones);


    this.send = function (message) {
        this.con.appendFromClient(message);
        this.WebSocketC.ws.send(message, function ack(error) {
            dl(error);
        });
    };
    this.onOpen = function () {
        //Hide connected and show disconnect button
        $(".connect").hide();
        $(".disconnect").show();
        //Enable send message
        $("#send").removeClass("disabled");
        window.AN.initialize(); 

        //ask for existing data
        this.MessageStringify.getData();
    };
    this.onMessage = function (e) {
        var message = e;
        this.con.appendFromServer(message);
        parser(message, this);
    };
    this.onClose = function () {
        $(".connect").show();
        $(".disconnect").hide();
        $("#send").addClass("disabled");
        this.Zones.removeAll();
    };
    this.onError = function (e) {
        var message = e;
        this.con.appendError(message);
    };
    this.onDisconnect = function () {
        this.Zones.removeAll();
    };
    
}