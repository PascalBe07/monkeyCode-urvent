var requestify = require('requestify');
var _ = require('underscore');

var cookieStore = {}
function parseCookies(request) {
    var list = {},
        rc = request.headers['set-cookie'];;
    rc && rc.forEach(function (cookie) {
        var parts = cookie.split('=');
        list[parts.shift()] = parts.shift().split(';')[0];
    });
    cookieStore = _.isEmpty(list) ? cookieStore : (Array.isArray(list) ? list[0] : list);
    return cookieStore;
}
var GET = function (address, callback) {
    //console.log(cookieStore);
    var invokeTime = new Date();
    requestify.get(address,
        {
            cookies: cookieStore,
            timeout: 40000
        }).then(function (response) {
            var diff = new Date() - invokeTime;
            console.log("Time: " + diff + " ms - " + response.getCode());
            parseCookies(response);

            callback(response.getBody());
        }).fail(function (response) {
            console.log("Error: " + response.getCode() + JSON.stringify(response));
            callback(null);
        });
}

module.exports = {
    get: GET
}