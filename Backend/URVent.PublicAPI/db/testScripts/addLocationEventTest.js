var http = require('http');
var requestify = require('requestify');

//var host = "http://urvent.ddns.net:3000"
//var apihost = "http://127.0.0.1:1337";
var apihost = "http://monkeycode.ddnss.de:3000";

var inseredEvents = 0;

var Id = function (length) {
    var text = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    for (var i = 0; i < length; i++)
        text += possible.charAt(Math.floor(Math.random() * possible.length));

    return text;
};


function work() {
    var lat = String(Math.random() * 100);
    var long = String(Math.random() * 100);
    var dist = Math.random() * 100;

    var CoverData = {
        "Url": "http://funnycats.blogin.com.au/wp-content/uploads/2015/01/aww-too-cute-cat.jpg",
        //"Url":"http://www.baysidecatresort.com.au/images/cute-cat-01.jpg",
        "ThumbnailLarge": null,
        "ThumbnailMedium": null,
        "ThumbnailSmall": null
    }

 

    var LocationData = {
        "Latitude": lat,
        "Longitude": long,
        "City": "AnyCity",
        "Street": "AnyStreet",
        "ZipCode": "AnyZipCode"
    };

    var EventData =
    {
        "Name": "Event " + Id(5),
        "Description": "AnyDescription",
        "EventTypeId": 1,
        "Price": 24,
        "StartDateTime": new Date(),
        "EndDateTime": new Date(),
        "cover": CoverData,
        "Priority": 1,
        "location": LocationData
    };


    UploadCover(CoverData, LocationData, EventData);
}

work();
setInterval(function () {
    work();
}, 400);

function UploadCover(CoverData, LocationData, EventData) {
    requestify.post(apihost + "/api/add/cover", CoverData)
        .then(function (res) {
            UploadLocationData(LocationData, EventData);
        })
        .fail(function (err) {
            //console.log(err);
            UploadLocationData(LocationData, EventData);
        });
}

function UploadLocationData(LocationData, EventData) {
    requestify.post(apihost + "/api/add/location", LocationData)
        .then(function (res) {
            EventData.location.Id = res.getBody().Id;
            UploadEventData(EventData);
        })
        .fail(function (err) {
            console.log("Location");
            console.log(err);
            UploadEventData(EventData);
        });
}

function UploadEventData(EventData) {
    requestify.post(apihost + "/api/add/event", EventData).then(function(res) {
            inseredEvents++;
            console.log("Created Events: " + inseredEvents);
        })
        .fail(function (err) {
            console.log("Event");
            console.log(err);
        });
}