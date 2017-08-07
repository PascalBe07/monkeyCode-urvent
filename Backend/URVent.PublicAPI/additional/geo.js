var Distance = function(lat1, long1, lat2, long2) {
    return Math.sqrt(Math.pow(71.5 * (long1 - long2), 2) + Math.pow((111.3 * (lat1 - lat2)), 2));
}

module.exports = {
    Distance: Distance
}