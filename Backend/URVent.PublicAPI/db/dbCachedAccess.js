var db = require('../db/db.js');
var _ = require('underscore');
var math_extensions = require('../additional/math_extensions.js');
var config = require('../__config.js');

var cache = {}
var pending = new Array();


Date.prototype.addDays = function (days) {
    var dat = new Date(this.valueOf());
    dat.setDate(dat.getDate() + days);
    return dat;
}

Date.prototype.cloneToUtc = function () {
    var dat = new Date(this.valueOf());
    return Date.UTC(dat.getFullYear(),
        dat.getMonth(),
        dat.getDate(),
        dat.getHours(),
        dat.getMinutes());
}

var generatePendingKey = function(lat, long, time) {
    return String(lat) + ":" + String(long) + ":" + time;
}

var simplifyLatLong = function (lat, long, cb) {
    process.nextTick(function () {
        //Round to 1 decimal place
        cb(math_extensions.Round(lat, 1), math_extensions.Round(long, 1));
        //cb(Math.round(lat), Math.round(long));
    });
}

var cacheSet = function (lat, long, time, data) {
    process.nextTick(function () {
        var key = generatePendingKey(lat, long, time);
        if (cache[key] == null) cache[key] = {}

        cache[key][time] = {}
        cache[key][time].timeStamp = new Date();
        cache[key][time].data = data;

        pending = _.without(pending, key);
    });
}

var cacheGet = function (lat, long, time, cb) {
    process.nextTick(function () {
        var key = generatePendingKey(lat, long, time);

        //Call Again if Pending
        if (_.contains(pending, key)) {
            setTimeout(function () { cacheGet(lat, long, time, cb); }, 500);
            return null;
        }

        if (cache[key] == null || cache[key][time] == null) {
            return cb(true, null);
        }

        var diff = (new Date() - new Date(cache[key][time].timeStamp)) / (1000 * 60);
        if (diff > config.refetch.globalTimeDelta) {
            return cb(true, cache[key][time].data);
        }

        return cb(false, cache[key][time].data);
    });
}

var getEvents = function (lat, long, time, cb) {
    process.nextTick(function () {
        simplifyLatLong(lat,
            long,
            function (lat, long) {
                cacheGet(lat, long, time,
                    function (refresh, data) {
                        var send = true;

                        //Send cached Data, set flag to not send it again after update
                        if (data != null) { send = false; cb(null, data); }

                        //If data is not outdated return
                        if (refresh === false) return;

                        process.nextTick(function () {
                            var key = generatePendingKey(lat, long, time);
                            pending.push(key);

                            var start_datetime = new Date();
                            var end_datetime = new Date();

                            //ToDo: Integrate timezone support
                            var day = start_datetime.getDay();

                            switch (time) {
                                case "today":
                                    end_datetime = new Date(start_datetime.getFullYear(),
                                        start_datetime.getMonth(),
                                        start_datetime.getDate(),
                                        23,
                                        59);
                                    break;

                                case "week":
                                    if (day < 7) {
                                        end_datetime = end_datetime.addDays(7 - day);
                                    }
                                    end_datetime.setHours(23);
                                    end_datetime.setMinutes(59);
                                    break;

                                case "weekend":
                                    if (day <= 5) {
                                        start_datetime = start_datetime.addDays(5 - day);
                                        start_datetime.setHours(day === 5 && start_datetime.getHours() >= 16 ? start_datetime.getHours() : 16);
                                        start_datetime.setMinutes(day === 5 && start_datetime.getHours() >= 16 ? start_datetime.getMinutes() : 0);
                                    }

                                    if (day < 7) {
                                        end_datetime = end_datetime.addDays(7 - day);
                                    }
                                    end_datetime.setHours(23);
                                    end_datetime.setMinutes(59);

                                    break;

                                case "all":
                                    end_datetime = end_datetime.addDays(365);
                                    end_datetime.setHours(23);
                                    end_datetime.setMinutes(59);
                                    break;
                                
                                default:
                                    break;
                            }

                            var eventsModel = db.model('events');
                            eventsModel.findAll({
                                limit: config.cache.size,
                                attributes: [
                                    'Id',
                                    'Name',
                                    'Description',
                                    'Price',
                                    'StartDateTime',
                                    'EndDateTime',
                                    [
                                        eventsModel.sequelize.fn('lat_lng_distance_simple',
                                        eventsModel.sequelize.col('Location_Latitude'),
                                        eventsModel.sequelize.col('Location_Longitude'),
                                        lat,
                                        long),
                                        'distance'
                                    ]
                                ],
                                include: [
                                    {
                                        model: db.model('covers'),
                                        attributes: ['Url']
                                    }
                                ],
                                having: {
                                    distance:
                                    {
                                        $lte: config.request.maxdistance
                                    },
                                    'StartDateTime':
                                    //Local Date in UTC umwandeln, da Datenbank nur lokale Zeiten kennt
                                    {
                                        $gte: start_datetime.cloneToUtc()
                                    },
                                    'EndDateTime': {
                                        $lte: end_datetime.cloneToUtc()
                                    }
                                },
                                order: [
                                    ['StartDateTime', 'ASC'],
                                    [eventsModel.sequelize.col('distance'), 'ASC']]
                            })
                                .then(function (data) {
                                    cacheSet(lat, long, time, data);
                                    if (send) cb(null, data);
                                });
                        });
                    });
            });
    });
}

module.exports = {
    GetEvents: getEvents
}


