var should = require('should');
var app = require('../src/assets/js/Models/WebSocketClient.js');
var handler = require('../src/assets/js/Models/WebSocketHandler.js');
var wHandler = handler._test.WebSocketHandler();
var WebSocketC = app._test.WebSocketClient("http://localhost:8080",wHandler);
console.log(WebSocketC);