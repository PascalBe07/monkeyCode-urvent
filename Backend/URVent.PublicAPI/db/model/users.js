/* jshint indent: 2 */

module.exports = function(sequelize, DataTypes) {
  return sequelize.define('users', {
    EMail: {
      type: DataTypes.STRING,
      allowNull: false,
      primaryKey: true
    },
    UrventUserData_Guid: {
      type: DataTypes.TEXT,
      allowNull: true
    },
    UrventUserData_AccessToken: {
      type: DataTypes.TEXT,
      allowNull: true
    },
    UrventUserData_AccessTokenReceived: {
      type: DataTypes.DATE,
      allowNull: false
    },
    Birthday: {
      type: DataTypes.DATE,
      allowNull: false
    },
    LastLogin: {
      type: DataTypes.DATE,
      allowNull: false
    },
    FacebookUserData_AccessToken: {
      type: DataTypes.TEXT,
      allowNull: true
    },
    FacebookUserData_AccessTokenReceived: {
      type: DataTypes.DATE,
      allowNull: false
    },
    Gender_Id: {
      type: DataTypes.INTEGER(11),
      allowNull: true,
      references: {
        model: 'genders',
        key: 'Id'
      }
    },
    UserEvent_EventId: {
      type: DataTypes.STRING,
      allowNull: true,
      references: {
        model: 'userevents',
        key: 'EventId'
      }
    },
    UserEvent_EventTypeId: {
      type: DataTypes.INTEGER(11),
      allowNull: true,
      references: {
        model: 'userevents',
        key: 'EventTypeId'
      }
    },
    UserEvent_UserEMail: {
      type: DataTypes.STRING,
      allowNull: true,
      references: {
        model: 'userevents',
        key: 'UserEMail'
      }
    }
  }, {
    tableName: 'users',
    timestamps: false
  });
};
