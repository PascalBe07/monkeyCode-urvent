const crypto = require('crypto');
var hash = crypto.createHash('sha256');

var Id = function (length) {
    var text = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    for (var i = 0; i < length; i++)
        text += possible.charAt(Math.floor(Math.random() * possible.length));

    return text;
};

var Hash = function (value) {
    hash = crypto.createHash('sha256');
    hash.update(value);
    return hash.digest('hex');
}

var HashAndRandom = function (value) {
    hash = crypto.createHash('sha256');
    value += Id(6);
    hash.update(value);
    return hash.digest('hex');
}

var HashAndRandomInt = function (value) {
    hash = crypto.createHash('sha256');
    value += Id(8);
    hash.update(value);
    var hashArray = hash.digest();
    hashArray = hashArray.slice(0, 6);
    return parseInt(hashArray.join(""));
}

var HashInt = function(value) {
    hash = crypto.createHash('sha256');
    hash.update(value);
    var hashArray = hash.digest();
    return parseInt(hashArray.join(""));
}


module.exports = {
    Id: Id,
    Hash: Hash,
    HashInt: HashInt,
    HashAndRandom: HashAndRandom,
    HashAndRandomInt: HashAndRandomInt
}