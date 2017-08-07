/* jshint indent: 2 */

module.exports = function(sequelize, DataTypes) {
  return sequelize.define('events', {
    Id: {
      type: DataTypes.STRING,
      allowNull: false,
      primaryKey: true
    },
    EventTypeId: {
      type: DataTypes.INTEGER(11),
      allowNull: false,
      primaryKey: true,
      references: {
        model: 'eventtypes',
        key: 'Id'
      }
    },
    Name: {
      type: DataTypes.TEXT,
      allowNull: false
    },
    Description: {
      type: DataTypes.TEXT,
      allowNull: false
    },
    Price: {
      type: 'DOUBLE',
      allowNull: true
    },
    AttendingCount: {
      type: DataTypes.INTEGER(11),
      allowNull: false
    },
    AttendingCountMale: {
      type: DataTypes.INTEGER(11),
      allowNull: false
    },
    AttendingCountFemale: {
      type: DataTypes.INTEGER(11),
      allowNull: false
    },
    StartDateTime: {
      type: DataTypes.DATE,
      allowNull: false
    },
    EndDateTime: {
      type: DataTypes.DATE,
      allowNull: false
    },
    Priority: {
      type: DataTypes.INTEGER(11),
      allowNull: false
    },
    Cover_Url: {
      type: DataTypes.STRING,
      allowNull: true,
      references: {
        model: 'covers',
        key: 'Url'
      }
    },
    Location_Id: {
      type: DataTypes.INTEGER(11),
      allowNull: true,
      references: {
        model: 'locations',
        key: 'Id'
      }
    },
    Location_Longitude: {
      type: DataTypes.FLOAT,
      allowNull: true,
      references: {
        model: 'locations',
        key: 'Longitude'
      }
    },
    Location_Latitude: {
      type: DataTypes.FLOAT,
      allowNull: true,
      references: {
        model: 'locations',
        key: 'Latitude'
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
    tableName: 'events',
    timestamps: false
  });
};
