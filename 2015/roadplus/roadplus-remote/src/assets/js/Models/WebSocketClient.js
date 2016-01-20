// assets/js/WebSocketClient.js
var WebSocket = require('ws');

function WebSocketClient(serverURI, handler) {
    this.STATE_DISCONNECTED = 0;
    this.STATE_CONNECTED = 1;
    this.STATE_CONNECTING = 2;
    this.STATE_CLOSED = 3;

    this.serverURI = serverURI;
    this.handler = handler;
    this.ws = new WebSocket(serverURI, {
        protocolVersion: 13,
        origin: 'http://127.0.0.1',
        userAgent: 'Webkit'
    });
    this.handler.WebSocketC = this;

    this.ws.on('open', function open() {
        handler.onOpen();
    });
    this.ws.on('message', function message(data, flags) {
        handler.onMessage(data);
        console.log(data, flags);
    });
    this.ws.on('close', function close() {
        handler.onClose();
    });
    this.ws.on('error', function error(e) {
        handler.onError(e);
        dl(e);

    });
    this.send = function (message) {
        if (this.ws.readyState == this.STATE_CONNECTED) {
            this.handler.send(message);
        } else {
            this.handler.onError("Not Connected");
        }
    };
    this.disconnect = function () {
        handler.onDisconnect();
        this.ws.close();
    };

}