var express = require('express');
var router = express.Router();
var session = require('express-session');
var cookieParser = require('cookie-parser');
var passport = require('passport');
var Strategy = require('passport-http-bearer').Strategy;
var auth = require('../auth/auth.js');
var dbImmediateAccess = require('../db/db.js');
var dbCachedAccess = require('../db/dbCachedAccess.js');
var id = require('../additional/idgenerator.js');
var geo = require('../additional/geo.js');
var log = require('../additional/log.js');
var _ = require('underscore');
var graphicsMagick = require('gm');
var config = require('../__config.js');

/* Require Parts */
require('./images')(router, dbImmediateAccess, graphicsMagick);
require('./locations')(router, dbImmediateAccess);

var sessionSecret = 'adf1e86fadFEAF1f3245$jl..-5+#dfa';

const AUTH_REQUIRED = false;

router.use(cookieParser(sessionSecret));
router.use(session({
    secret: sessionSecret,
    resave: false,
    saveUninitialized: false,
    maxAge: 120000
}));

router.use(passport.initialize());
router.use(passport.session());


passport.use(new Strategy(
    function (token, done) {
        auth.findByToken(token,
            function (err, user) {
                if (err) return done(error);
                if (!user) return done(null, false);
                return done(null, user); 
            });
    }));

passport.serializeUser(function (user, done) {
    done(null, user.id);
});

passport.deserializeUser(function (id, done) {
    auth.findById(id, function (err, user) {
        done(err, user);
    });
});


// Authenticate for any request, except in nonSecurePaths
router.use(function (req, res, next) {
    if (config.security.authRequired) {
        var nonSecurePaths = ['/', '/login', '/heartbeat', '/register', '/add/location', '/add/event'];

        if (_.contains(nonSecurePaths, req.path)) {
            return next();
        }
        //authenticate user
        /*passport.authenticate('bearer', { session: true }),*/

        if (req.session.login == null || req.session.login.token == null) {
            return res.send("Invalid Session");
        }
    }

    next();
});


/* GET home page. */
router.get('/',
    function (req, res) {
        res.send("URVENT_PUBLIC_API");
    });


router.get('/heartbeat',
    function (req, res) {
        if (req.session != null) {
            if (req.session.count == null) { req.session.count = -1 }
            req.session.count = (++req.session.count) % 10;
            return res.send(String(req.session.count));
        }
        res.send(new Date());
    });


function GetSettings(req, res, next) {
    if (req.session.settings == null && req.session.login != null ) {
        dbImmediateAccess.model('settings')
            .findOne({
                where: { 'Id': req.session.login.uid }
            })
            .then(function (data) {
                if (data == null) {
                    req.session.settings = {};
                    req.session.settings.MaxDistance = 1000;
                } else {
                    req.session.settings = data.toJSON();
                }   
                    next(req, res, next);
            });
    }
    else {
        next(req, res,next);
    }
}

// Check if Database Call Required
router.get('/get/events/:time',
    function (req, res, next) {
        var time = req.params.time.toLowerCase();
        var session = req.session;
        if (session != null) {
            if (req.session.lastCall == null || req.session.lastCall == undefined) {
                req.session.lastCall = {};
            }
            if (req.session.lastCall[time] == null || req.session.lastCall[time] == undefined)
                req.session.lastCall[time] = new Object();
            else {   
                var diffTime = (new Date - new Date(req.session.lastCall[time].timeStamp)) / (1000 * 60);
                var diffDistance = geo.Distance(req.query.lat,
                    req.query.long,
                    req.session.lastCall[time].lat,
                    req.session.lastCall[time].long);

                if (diffTime < config.refetch.timeDelta && diffDistance < config.refetch.minDistance)
                    if (req.session.lastCall[time].result)
                        return res.send(req.session.lastCall[time].result);
            }
            req.session.lastCall[time].lat = req.query.lat;
            req.session.lastCall[time].long = req.query.long;
            req.session.lastCall[time].timeStamp = new Date();
        }
        
        next();
    });

router.get('/get/events/:time', //+Params
    function (req, res) {
        var time = req.params.time.toLowerCase();
        GetSettings(req,
            res,
            function (req, res) {
                dbCachedAccess.GetEvents(req.query.lat,
                    req.query.long,
                    time,
                    function (err, data) {
                        if (err) {
                            log.logger.error(err);
                            return res.status(404).send("No Events found");
                        }
                        if (data != null && data.length > 0) {
                            req.session.lastCall[time].result = data;
                            return res.send(data);
                        }
                    return res.status(404).send("No Events found");
                    });
            });

    });

router.get('/distance/:lat/:long', //+Params
    function (req, res) {
        dbImmediateAccess.seq().query("SELECT `urvent`.`lat_lng_distance`(:lat1, :long1, 49.4440558, 11.8454068) AS Distance_To_OTH_Amberg",
            { replacements: { lat1: req.params.lat, long1: req.params.long } })
            .spread(function (results, metadata) {
                res.send(results[0]);
            });
    });

router.get('/set/personalevents/:uid', //Hier wird noch json mitgegeben
    function (req, res) {

    });

router.get('/:uid/set/fbtoken/:token',
    function (req, res) {

    });


router.get('/login',
    function (req, res) {
        dbImmediateAccess.model('users')
            .findOne({
                where: { UrventUserData_Guid: req.query.uid },
                attributes: [['UrventUserData_AccessToken','token'],['UrventUserData_AccessTokenReceived', 'received']]
            })
            .then(function(user) {
                if (user == null) {
                    return res.status(404).send("Uid not found");
                }
                if (req.session.login == null) {
                    req.session.login = {};
                    req.session.login.timeStamp = new Date();
                }

                req.session.login.uid = req.query.uid;


                var data = user.get();
                var token = data.token;
                var diff = new Date() - new Date(data.received);
                diff = diff / (1000 * 60 * 60); //Diff in Hours
                if (diff < config.security.tokenExpiration) {
                    req.session.login.token = token;
                    req.session.login.tokenTimeStamp = data.UrventUserData_AccessTokenReceived;
                    return res.send('OK');
                } else {
                    req.session.login.token = null;
                    req.session.login.tokenTimeStamp = null;
                    return res.status(403).send("Token expired");
                }
            });
    });

router.get("/get/token",
    function(req, res) {
        var token = id.Id(16);
        if (req.session != null && req.session.login.uid != null && req.query.uid === req.session.login.uid) {
            var diff = (new Date() - new Date(req.session.login.tokenTimeStamp)) / (1000 * 60 * 60 );
            if (req.session.login.tokenTimeStamp === null || diff >= config.security.tokenExpiration) { 
                dbImmediateAccess.model('users')
                    .findOne({
                        where: {
                            UrventUserData_Guid: req.query.uid,
                            UrventUserData_AccessToken: req.query.token
                        },
                        attributes: [
                            'EMail',
                            'UrventUserData_AccessToken',
                            'UrventUserData_AccessTokenReceived'
                        ]
                    })
                    .then(function(user) {
                        if (user != null) {
                            user.update({
                                UrventUserData_AccessToken: token,
                                UrventUserData_AccessTokenReceived: new Date()
                            });
                        }
                    });
            }
        } 
            return res.send(token);
    });

router.post('/register',
    function(req, res) {
        dbImmediateAccess.model('users')
            .findOrCreate({
                attributes: ['UrventUserData_Guid','UrventUserData_AccessToken','Gender_Id','Birthday'],
                where: { EMail: req.body.EMail},
                defaults: {
                    UrventUserData_Guid: id.HashAndRandom(req.body.EMail),
                    Gender_Id: req.body.Gender_Id,
                    Birthday: req.body.Birthday,
                    LastLogin: new Date(),
                    UrventUserData_AccessToken: id.Id(16),
                    UrventUserData_AccessTokenReceived: new Date(),
                    FacebookUserData_AccessToken: null,
                    FacebookUserData_AccessTokenReceived: new Date()
                }
            })
            .spread(function (user, created) {
                var user = user.get();

                if (String(new Date(req.body.Birthday)) !== String(user.Birthday) || req.body.Gender_Id != user.Gender_Id) {
                    return res.status(403).send('Invalid Userdata');
                }
        
                var answer = new Object();
                answer['UrventUserData_Guid'] = user['UrventUserData_Guid'];
                answer['UrventUserData_AccessToken'] = user['UrventUserData_AccessToken'];

                res.send(answer);
            }).catch(function (err) {
                log.logger.error(err);
                return res.status(400).send("Incorrect Data Format");
            });
    });


router.get('/set/interest',
    function(req, res, next) {
        var evid = req.query.eventid;
    });


router.post('/add/event',
    function (req, res) {
        var local_id = id.Hash(req.body.name + req.body.StartDateTime + req.body.location.Id);
        dbImmediateAccess.model('events')
            .findOrCreate({
                where: {
                    Id: local_id
                },
                defaults: {
                    Id: local_id,
                    EventTypeId: req.body.EventTypeId,
                    Name: req.body.Name,
                    Description: req.body.Description,
                    Price: req.body.Price,
                    AttendingCount: 0,
                    AttendingCountMale: 0,
                    AttendingCountFemale: 0,
                    StartDateTime: req.body.StartDateTime,
                    EndDateTime: req.body.EndDateTime,
                    Priority: req.body.Priority,
                    Cover_Url: unescape(req.body.cover.Url),
                    Location_Id: req.body.location.Id,
                    Location_Latitude: req.body.location.Latitude,
                    Location_Longitude:req.body.location.Longitude
                }
            })
            .spread(function(location, created) {
                res.status(created ? 200 : 403).send(created);
            }).catch(function (err) {
                log.logger.error(err);
                res.status(500).send(err);
            });
    });

module.exports = router;