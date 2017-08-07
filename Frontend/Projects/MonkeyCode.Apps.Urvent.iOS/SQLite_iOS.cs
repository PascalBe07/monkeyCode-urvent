using System;
using System.IO;
using MonkeyCode.Apps.Urvent.iOS;
using MonkeyCode.Apps.Urvent.Portable.Application;
using SQLite.Net.Interop;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_iOS))]

namespace MonkeyCode.Apps.Urvent.iOS
{
    public class SQLite_iOS : ISQLite
    {
        public SQLite_iOS()
        {
        }

        #region ISQLite implementation

        public SQLite.Net.SQLiteConnection GetConnection()
        {
            var fileName = "Urvent.db3";
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.Combine(libraryPath, fileName);

            var platform = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS();
            var connection = new SQLite.Net.SQLiteConnection(platform, path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);

            return connection;
        }

        #endregion
    }
}