/* jshint indent: 2 */

module.exports = function(sequelize, DataTypes) {
  return sequelize.define('settings', {
    Id: {
      type: DataTypes.INTEGER(11),
      allowNull: false,
      primaryKey: true,
      autoIncrement: true
    },
    MaxDistance: {
      type: 'DOUBLE',
      allowNull: false
    },
    AttractedTo_Id: {
      type: DataTypes.INTEGER(11),
      allowNull: true,
      references: {
        model: 'genders',
        key: 'Id'
      }
    },
    EventDateRange_Id: {
      type: DataTypes.INTEGER(11),
      allowNull: true,
      references: {
        model: 'eventdateranges',
        key: 'Id'
      }
    }
  }, {
    tableName: 'settings',
    timestamps: false
  });
};
