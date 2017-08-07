/* jshint indent: 2 */

module.exports = function(sequelize, DataTypes) {
  return sequelize.define('eventtypes', {
    Id: {
      type: DataTypes.INTEGER(11),
      allowNull: false,
      primaryKey: true,
      autoIncrement: true
    },
    Type: {
      type: DataTypes.TEXT,
      allowNull: true
    }
  }, {
    tableName: 'eventtypes',
    timestamps: false
  });
};
