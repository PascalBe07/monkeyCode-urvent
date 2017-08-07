/* jshint indent: 2 */

module.exports = function(sequelize, DataTypes) {
  return sequelize.define('userevents', {
    EventId: {
      type: DataTypes.STRING,
      allowNull: false,
      primaryKey: true,
      references: {
        model: 'events',
        key: 'Id'
      }
    },
    EventTypeId: {
      type: DataTypes.INTEGER(11),
      allowNull: false,
      primaryKey: true,
      references: {
        model: 'events',
        key: 'EventTypeId'
      }
    },
    UserEMail: {
      type: DataTypes.STRING,
      allowNull: false,
      primaryKey: true,
      references: {
        model: 'users',
        key: 'EMail'
      }
    },
    Status_Id: {
      type: DataTypes.INTEGER(11),
      allowNull: true,
      references: {
        model: 'usereventstatus',
        key: 'Id'
      }
    }
  }, {
    tableName: 'userevents',
    timestamps: false
  });
};
