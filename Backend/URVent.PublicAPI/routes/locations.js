var idgen = require("../additional/idgenerator.js");
var log = require('../additional/log.js');

module.exports = function (router, dbImmediateAccess) {

    router.post('/add/location',
        function (req, res) {
            var id = idgen.HashInt(String(parseFloat(req.body.Latitude)) + String(parseFloat(req.body.Longitude)) + String(req.body.City ||"") + String(req.body.Street ||"") + String(req.body.ZipCode || "")) % 2147483646;
            dbImmediateAccess.model('locations')
                .findOrCreate({
                    where: {
                        Id: id
                    },
                    defaults: {
                        Id: id,
                        Latitude: parseFloat(req.body.Latitude),
                        Longitude: parseFloat(req.body.Longitude),
                        City: String(req.body.City),
                        Street: String(req.body.Street),
                        ZipCode: String(req.body.ZipCode)
                    }
                })
                .spread(function(location, created) {
                    res.status(created ? 200 : 403).send(location);
                })
                .catch(function(error) {
                    log.logger.error(error);
                    res.status(500).send(error);
                });
        });

}