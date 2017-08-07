var express = require('express');
var path = require('path');
//var favicon = require('serve-favicon');

var bodyParser = require('body-parser');
var log = require('./additional/log.js');
var logger = require('morgan');
var routes = require('./routes/index');
//var users = require('./routes/users');


var app = express();

//var sessionSecret = 'adf1e86fadFEAF1f3245$jl..-5+#dfa';

// view engine setup
//app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'jade');
// uncomment after placing your favicon in /public
//app.use(favicon(__dirname + '/public/favicon.ico'));
//app.use(logger('dev'));
app.use(logger("combined", { "stream": log.logger.stream }));
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: false }));

app.use(require('stylus').middleware(path.join(__dirname, 'public')));
app.use(express.static(path.join(__dirname, 'public')));

//app.use(express.static('dist'));
app.use('/api', routes);

app.get('/doc',
    function(req, res) {
        res.sendFile(__dirname + '/doc/doc.html');
    });

app.get('/doc/json',
    function(req, res) {
        res.sendFile(__dirname + '/doc/api.json');
    });

app.get('/doc/redoc',
    function(req, res) {
        res.sendFile(__dirname + '/doc/redoc.min.js');
    });
//app.use('/users', users);

// catch 404 and forward to error handler
app.use(function (req, res, next) {
    var err = new Error('Not Found');
    err.status = 404;
    next(err);
});

// error handlers

// development error handler
// will print stacktrace
if (app.get('env') === 'development') {
    app.use(function (err, req, res, next) {
        res.status(err.status || 500);
        //res.render('error', {
        //    message: err.message,
        //    error: err
        //});
        res.send(err.message);
    });
}

// production error handler
// no stacktraces leaked to user
app.use(function (err, req, res, next) {
    res.status(err.status || 500);
    //res.render('error', {
    //    message: err.message,
    //    error: {}
    //});
    res.send(JSON.stringify(err));
});


module.exports = app;
