CREATE FUNCTION `lat_lng_distance_simple` (lat1 FLOAT, lng1 FLOAT, lat2 FLOAT, lng2 FLOAT)
    RETURNS FLOAT
    DETERMINISTIC
    BEGIN
		RETURN SQRT(POWER((71.5 * (lng1 - lng2)),2) + POWER((111.3 * (lat1 -lat2)),2));
    END
