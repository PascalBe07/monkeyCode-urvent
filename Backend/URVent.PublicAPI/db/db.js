/*
MySQL

-> Sequelize http://docs.sequelizejs.com/en/latest/
-> Sequelize-Auto https://github.com/sequelize/sequelize-auto
*/

//private
var fs = require('fs');
var initialized = false;
var path = require('path');
var Sequelize = require('sequelize');
var config = require('../__config.js');
var logInstance = require('../additional/log.js');
var db = {};

var singleton = function singleton(initialized_callback) {
    var sequelize = new Sequelize(config.database.dbname,
        config.database.user,
        config.database.password,
        {
            host: config.database.host,
            dialect: 'mysql',
            pool: {
                max: 5,
                min: 0,
                idle: 10000
            },
            benchmark: true,
            logging:function(log) {
                logInstance.logger.log('debug', log);
            }
        });
    sequelize.authenticate()
        .then(function (err) {
            logInstance.logger.info('Database connection has been established successfully.');
            initialized = true;
        }).then(function() {
                //sequelize.sync();
            })
        .catch(function(err) {
            logInstance.logger.error('Unable to connect to the database:', err);
            return;
        });

    fs.readdir(path.join(__dirname,'model'),
        function (err, files) {
            if (err) {
                logInstance.logger.error("Could not list Sequelize Import Directory", err);
                initialized = false;
                return;
            }
            //Iterate Model Definitions and Import them
            files.forEach(function (file, index) {
                if (path.extname(file) === ".js" && !(file.substring(0,1) === "_")) {               
                    var model = sequelize["import"](path.join(__dirname, "model", file));
                    logInstance.logger.log('debug',"Imported: " + file + " as: " + model.name);
                    db[model.name] = model;
                }
            });
            db["events"].belongsTo(db["covers"], { foreignKey: 'Cover_Url'});           
            db["events"].belongsTo(db["locations"], { foreignKey: 'Location_Id'}); //ToDo: Foreign Key ersetzen!
            

            db.sequelize = sequelize;
            db.Sequelize = Sequelize;
        });

    this.model = function (name) {
        return db[name];
    }

    this.Seq = function () {
        return Sequelize;
    }

    this.seq = function() {
        return sequelize;
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
