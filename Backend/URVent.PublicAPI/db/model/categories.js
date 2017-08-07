/* jshint indent: 2 */

module.exports = function(sequelize, DataTypes) {
  return sequelize.define('categories', {
    Id: {
      type: DataTypes.INTEGER(11),
      allowNull: false,
      primaryKey: true,
      autoIncrement: true
    },
    Name: {
      type: DataTypes.TEXT,
      allowNull: false
    },
    Event_Id: {
      type: DataTypes.STRING,
      allowNull: true,
      references: {
        model: 'events',
        key: 'Id'
      }
    },
    Event_EventTypeId: {
      type: DataTypes.INTEGER(11),
      allowNull: true,
      references: {
        model: 'events',
        key: 'EventTypeId'
      }
    },
    Setting_Id: {
      type: DataTypes.INTEGER(11),
      allowNull: true,
      references: {
        model: 'settings',
        key: 'Id'
      }
    }
  }, {
    tableName: 'categories',
    timestamps: false
  });
};
