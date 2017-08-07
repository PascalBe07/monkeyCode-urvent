/* jshint indent: 2 */

module.exports = function(sequelize, DataTypes) {
  return sequelize.define('covers', {
    Url: {
      type: DataTypes.STRING,
      allowNull: false,
      primaryKey: true
    },
    ThumbnailLarge: {
      type: 'LONGBLOB',
      allowNull: true
    },
    ThumbnailMedium: {
      type: 'LONGBLOB',
      allowNull: true
    },
    ThumbnailSmall: {
      type: 'LONGBLOB',
      allowNull: true
    }
  }, {
    tableName: 'covers',
    timestamps: false
  });
};
