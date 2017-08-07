namespace MonkeyCode.Framework.Portable.Urvent.Services
{
    public interface IDataService
    {
        //  track changes with datetime

        void AddItem<T>(T item);

        T[] GetItems<T>() where T : class;

        //bool DeleteItem<T>(T item);

    }
}