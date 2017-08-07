//using System.Collections.Generic;

//namespace MonkeyCode.Apps.Urvent.Portable.Models
//{
//    public class SQlitePclDataService : IDataService
//    {
//        private SQLiteConnection sqliteConnection;
//        public SQlitePclDataService(SQLiteConnection sqliteCon)
//        {
//            Connect(sqliteCon);
//        }

//        public bool Connect(SQLiteConnection sqliteCon)
//        {
//            if (sqliteCon == null)
//                   return false;
//            this.sqliteConnection = sqliteCon;
//            return this.sqliteConnection != null;
//        }

//        public bool CreateTable<T>() where T : class
//        {
//            return this.sqliteConnection.CreateTable<T>() > 0;
//        }


//        public void AddItem<T>(T item) 
//        {
//            //this.sqliteConnection.UpdateWithChildren(item);
//            this.sqliteConnection.RunInTransaction(() =>
//            {
//                this.sqliteConnection.InsertOrReplaceWithChildren(item, true);
//            });
            
//        }

//        public void AddItems<T>(IEnumerable<T> items)
//        {
//            //foreach(var item in items)
//            //    this.sqliteConnection.UpdateWithChildren(item);

//            //http://stackoverflow.com/questions/36774326/poor-performance-sqlite-net-extensions
//            this.sqliteConnection.RunInTransaction(() =>
//            {
//                this.sqliteConnection.InsertOrReplaceAllWithChildren(items, true);
//            });
//        }

//        public T[] GetItems<T>() where T : class
//        {
//            return this.sqliteConnection.GetAllWithChildren<T>(null,true).ToArray();
//        }

//        /*
//        public T GetItem<T,TPk>(TPk primaryKey) where T : class
//        {
//            return this.sqliteConnection.Get<T>(primaryKey);
//        }
//        */

//    }
//}
