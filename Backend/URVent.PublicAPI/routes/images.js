var request = require('request').defaults({ encoding: null });
var path = require('path');
var log = require('../additional/log.js');

module.exports = function (router, dbImmediateAccess, graphicsMagick) {

    router.post('/add/cover',
        function (req, res) {
            var url = unescape(req.body.Url);
            dbImmediateAccess.model('covers')
                .findOrCreate({
                    where: {
                        Url: url
                    },
                    defaults: {
                        Url: url
                    }
                })
                .spread(function (cover, created) {
                    //res.status(created ? 200 : 403).send(created);
                    if (!created) {
                         return res.status(403).send(created);
                    }
                    request(req.body.Url,
                        function (error, response, data) {
                            var sent = false;
                            var sendError = () => {
                                if (!sent) {
                                    sent = true;
                                    dbImmediateAccess.model('covers')
                                        .destroy({
                                            where: {
                                                Url: url
                                            }
                                        });
                                    res.status(500).send("Invalid Image");
                                }
                            }
                            var sendOk = () => {
                                if (!sent) {
                                    sent = true;
                                    res.status(200).send(true);
                                }
                            };

                            if (error == null) {
                                //If no Error -> URL seems to be correct
                                    graphicsMagick(data, path.basename(url))
                                    .resize(640, 640)
                                    .toBuffer('PNG',
                                        function(err, buffer) {
                                            if (err) {
                                                sendError();
                                                return;
                                            }
                                            cover.update({
                                                "ThumbnailLarge": buffer
                                            });
                                            sendOk();
                                        });

                                graphicsMagick(data, path.basename(url))
                                    .resize(320, 320)
                                    .toBuffer('PNG',
                                        function(err, buffer) {
                                            if (err) {
                                                sendError();
                                                return;
                                            }
                                            cover.update({
                                                "ThumbnailMedium": buffer
                                            });
                                            sendOk();
                                        });

                                graphicsMagick(data, path.basename(url))
                                    .resize(80, 80)
                                    .toBuffer('PNG',
                                        function(err, buffer) {
                                            if (err) {
                                                sendError();
                                                return;
                                            }
                                            cover.update({
                                                "ThumbnailSmall": buffer
                                            });
                                            sendOk();
                                        });
                            } else {
                                sendError();
                            }
                        });
                });
        });

    router.get("/get/thumbnail",
        function(req, res) {
            var size;
            switch (req.query.Size) {
            case "Large":
                size = "ThumbnailLarge";
                break;
            case "Medium":
                size = "ThumbnailMedium";
                break;
            case "Small":
                size = "ThumbnailSmall";
                break;
            default:
                return res.status(403).send("Invalid Size");
            }

            dbImmediateAccess.model('covers')
                .findOne({
                    where: { Url: req.query.Url },
                    attributes: [size]
                })
                .then(function(Image) {
                    if (!Image) return res.status(404).send("Cover Not Found");
                    res.writeHead(200, { 'Content-Type': 'image/png' });
                    res.end(Image.get(size), 'binary');
                });
        });
}