var winston = require('winston');



var singleton = function singleton() {

    this.logger = new winston.Logger({
        level: 'verbose',
        transports: [
            new winston.transports.Console({
                level: 'info',
                handleExceptions: true,
                json: false,
                colorize: true
            }),
            new winston.transports.File({
                name:"info-file",
                level: 'info',
                filename : './logs/log.log',
                handleExceptions: true,
                json: false,
                maxsize: 500000, //5MB
                maxFiles: 5,
                colorize: false
            }),
            new winston.transports.File({
                name:"debug-file",
                level: 'debug',
                filename: './logs/debug.log',
                handleExceptions: true,
                json: false,
                maxsize: 500000, //5MB
                maxFiles: 1,
                colorize: false
            })
        ],
        exitOnError: false
    });

    var logger = this.logger;

    this.logger.stream = {
        write: function(message, encoding) {
            logger.info(message);
        }
    }

}
singleton.instance = null;

singleton.getInstance = function () {
    if (this.instance === null) {
        this.instance = new singleton();
    }
    return this.instance;
}
module.exports = singleton.getInstance();
