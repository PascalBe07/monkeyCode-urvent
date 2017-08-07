using SQLite.Net;

namespace MonkeyCode.Apps.Urvent.Portable.Application
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
