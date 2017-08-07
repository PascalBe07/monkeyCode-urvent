/* jshint indent: 2 */

module.exports = function(sequelize, DataTypes) {
  return sequelize.define('eventdateranges', {
    Id: {
      type: DataTypes.INTEGER(11),
      allowNull: false,
      primaryKey: true,
      autoIncrement: true
    },
    Description: {
      type: DataTypes.TEXT,
      allowNull: true
    }
  }, {
    tableName: 'eventdateranges',
    timestamps: false
  });
};
