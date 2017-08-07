/* jshint indent: 2 */

module.exports = function(sequelize, DataTypes) {
  return sequelize.define('locations', {
    Id: {
      type: DataTypes.INTEGER(11),
      allowNull: false,
      primaryKey: true
    },
    Longitude: {
      type: DataTypes.FLOAT,
      allowNull: false,
      primaryKey: true
    },
    Latitude: {
      type: DataTypes.FLOAT,
      allowNull: false,
      primaryKey: true
    },
    City: {
      type: DataTypes.TEXT,
      allowNull: true
    },
    Street: {
      type: DataTypes.TEXT,
      allowNull: true
    },
    ZipCode: {
      type: DataTypes.TEXT,
      allowNull: true
    }
  }, {
    tableName: 'locations',
    timestamps: false
  });
};
