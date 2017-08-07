var users = [{ id: 1, username: 'default', token: 'default' }];

//Hier Datenbank für Token
exports.findByToken = function (token, cb) {
    process.nextTick(function () {
        for (var i = 0, len = users.length; i < len; i++) {
            var record = users[i];
            if (record.token === token) {
                return cb(null, record);
            }
        }
        return cb(null, null);
    });
}

exports.findById = function(id, cb) {
    process.nextTick(function () {
        for (var i = 0, len = users.length; i < len; i++) {
            var record = users[i];
            if (record.id === id) {
                return cb(null, record);
            }
        }
        return cb(null, null);
    });
}