/* jshint indent: 2 */

module.exports = function(sequelize, DataTypes) {
  return sequelize.define('__migrationhistory', {
    MigrationId: {
      type: DataTypes.STRING,
      allowNull: false,
      primaryKey: true
    },
    ContextKey: {
      type: DataTypes.STRING,
      allowNull: false
    },
    Model: {
      type: 'LONGBLOB',
      allowNull: false
    },
    ProductVersion: {
      type: DataTypes.STRING,
      allowNull: false
    }
  }, {
    tableName: '__migrationhistory',
    timestamps: false
  });
};
