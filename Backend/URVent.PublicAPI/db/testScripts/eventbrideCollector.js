var request = require('request');

var town = "Deutschland";

var pagelimit = 99;
var pageIndex = 1;
var url = "https://www.eventbriteapi.com/v3/events/search/?location.address=" + town + "&token=GZW3GIZUGH7U7GEK2QQW&expand=organizer,venue";
var events = [];

getEvents(1);

function getEvents(pageIndex) {
    request(url + "&page=" + pageIndex,
        function(error, response, body) {
            if (!error && response.statusCode === 200) {

                var body = JSON.parse(body);

                for (event in body.events) {
                    events.push(body.events[event]);
                }

                if (body.pagination.page_count > pageIndex && pageIndex + 1 < pagelimit) {
                    console.log("Total Downloaded Events: " + events.length + " -- page " + pageIndex + " of " + body.pagination.page_count);
                    getEvents(pageIndex + 1); 
                }
                else {
                    pageIndex = 0;
                    uploadEvent(0);
                }    
            }
        });
}

function uploadEvent(eventIndex) {
    var event = events[eventIndex];
    var cover_url = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2b/No_image_available-de.svg/600px-No_image_available-de.svg.png";
    if (event.logo && event.logo.original && event.logo.original.url) {
        cover_url = event.logo.original.url;   
    }
    var CoverData = {
        "Url": cover_url,
        "ThumbnailLarge": null,
        "ThumbnailMedium": null,
        "ThumbnailSmall": null
    }

    var LocationData = {
        "Latitude": parseFloat(event.venue.address.latitude),
        "Longitude": parseFloat(event.venue.address.longitude),
        "City": event.venue.address.city || null,
        "Street": event.venue.address.street || null,
        "ZipCode": event.venue.address.postal_code || null
    };

    var EventData =
        {
            "Name": event.name.text,
            "Description": event.description.text || "No Description",
            "EventTypeId": 1,
            "Price": 0,
            "StartDateTime": event.start.local,
            "EndDateTime": event.end.local,
            "cover": CoverData,
            "Priority": 1,
            "location": LocationData
        };

    doUpload(CoverData,
        LocationData,
        EventData,
        function(error) {
            if (eventIndex < events.length - 1) {
                uploadEvent(eventIndex + 1);
                return;
            }
            console.log("done");
        });
}


var requestify = require('requestify');
var apihost = "http://h2622931.stratoserver.net:3000";
//var apihost = "http://127.0.0.1:1337";

function doUpload(CoverData, LocationData, EventData, callback) {
    UploadCover(CoverData, LocationData, EventData, function(error) {
        callback(error);
    });
}

function UploadCover(CoverData, LocationData, EventData, callback) {
    requestify.post(apihost + "/api/add/cover", CoverData)
        .then(function (res) {
            UploadLocationData(LocationData, EventData, callback);
        })
        .fail(function (err) {
            handleError(err, "Cover");
            UploadLocationData(LocationData, EventData, callback);
        });
}

function UploadLocationData(LocationData, EventData, callback) {
    requestify.post(apihost + "/api/add/location", LocationData)
        .then(function (res) {
            EventData.location.Id = res.getBody().Id;
            UploadEventData(EventData, callback);
        })
        .fail(function (err) {
            handleError(err, "Location");
            EventData.location.Id = err.getBody().Id;
            UploadEventData(EventData, callback);
        });
}

function UploadEventData(EventData, callback) {
    requestify.post(apihost + "/api/add/event", EventData).then(function (res) {
            successMessage(EventData.Name);
            callback(null);
        })
        .fail(function (err) {
            handleError(err, "Event");
            successMessage(EventData.Name);
            callback(null);
        });
}

function successMessage(name) {
    console.log("Uploaded Event: " + name + " (" + ++pageIndex + "/" + events.length+1 + ")");
}

function handleError(err, type) {
    try {
        var code = err.getCode();
        if (code === 403) {
            console.log(type + " already existed");
        } else if (code === 500) {
            console.log("Internal Error " + err);
        } else {
            console.log("Unknown Error " + err);
        }
    } catch (err) {
        console.log(err);
    }
}