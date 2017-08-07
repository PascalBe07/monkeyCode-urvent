
// var http = require('http');
var http = require("../../additional/urventRequest.js");


//var host = "http://urvent.ddns.net:3000/api";
//var host = "http://monkeycode.ddnss.de:3000/api";
var host = "http://localhost:1337";
var path = "/api/get/events/today?lat=";
//var path = "/heartbeat";

setInterval(function() {
    //var invokeTime = new Date();
    //console.log("Invoke Request");
    
    var long = Math.floor((Math.random() * 25) + 1); 
    var lat = Math.floor((Math.random() * 25) + 1); 
    var currPath = path + lat + "&long=" + long;
        http.get(host + currPath,
            function(res) {
            });
    },
    5000);
