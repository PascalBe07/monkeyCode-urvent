using System;
using System.IO;
using Xamarin.Forms;
using MonkeyCode.Apps.Urvent.Droid;
using MonkeyCode.Apps.Urvent.Portable.Application;
using SQLite.Net.Interop;

[assembly: Dependency(typeof(SQLite_Android))]
 
namespace MonkeyCode.Apps.Urvent.Droid
{
    public class SQLite_Android : ISQLite
    {
        public SQLite_Android()
        {
        }

        #region ISQLite implementation

        public SQLite.Net.SQLiteConnection GetConnection()
        {
            var fileName = "Urvent.db3";
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, fileName);

            var platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
            var connection = new SQLite.Net.SQLiteConnection(platform, path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);

            return connection;
        }

        #endregion
    }
}