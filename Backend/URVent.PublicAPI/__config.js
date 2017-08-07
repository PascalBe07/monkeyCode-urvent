var config = {};

config.database = {};
config.database.user = "UrventAdmin";
config.database.password = "Urvent.2016";
config.database.host = "h2622931.stratoserver.net";
config.database.dbname = 'urvent';

/* Cache */
config.refetch = {};
config.refetch.minDistance = 5; // km
config.refetch.timeDelta = 10; // Private Cache Expiration Minutes
config.refetch.globalTimeDelta = 30; // Global Cache Expiration Minutes

config.cache = {};
config.cache.size = 100;


/* Request */
config.request = {}
config.request.maxdistance = 80; //approx. airline km, calculated by db

/* Security */
config.security = {};
config.security.tokenExpiration = 24; //Hours
config.security.authRequired = false;





module.exports = config;


