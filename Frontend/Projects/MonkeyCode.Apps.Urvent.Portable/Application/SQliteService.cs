using System;
using System.Collections.Generic;
using MonkeyCode.Apps.Urvent.Portable.Models;
using MonkeyCode.Framework.Portable.Urvent.Models;
using MonkeyCode.Framework.Portable.Urvent.Models.Mobile;
using MonkeyCode.Framework.Portable.Urvent.Services;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using Xamarin.Forms;

namespace MonkeyCode.Apps.Urvent.Portable.Application
{
    public class SQliteService : IDataService
    {
        private readonly SQLiteConnection sqliteConnection;
        private readonly IModelConverterService converterService;

        public SQliteService(IModelConverterService converter)
        {
            sqliteConnection = DependencyService.Get<ISQLite>().GetConnection();
            sqliteConnection.CreateTable<MobileCategory>();
            sqliteConnection.CreateTable<MobileEvent>();
            sqliteConnection.CreateTable<MobileUser>();

            converterService = converter;
        }

        public bool CreateTable<T>() where T : class
        {
            return this.sqliteConnection.CreateTable<T>() > 0;
        }


        public void AddItem<T>(T item)
        {
            Type requestType = typeof(T);
            MobileEntity mobEntity = null;
            try
            {
                if (requestType == typeof (SettingsContainer))
                {
                    //Todo use AutoMapper
                    SettingsContainer settingContainer = item as SettingsContainer;
                    if (settingContainer != null)
                    {
                        lock (settingContainer)
                        {
                            MobileUser mobUser = new MobileUser();
                            Tuple<double, double> geolocation = settingContainer.GetGeolocation();
                            mobUser.Latitude = geolocation.Item1;
                            mobUser.Longitude = geolocation.Item2;
                            mobUser.EventDateRange = settingContainer.GetEventDateRange();
                            mobUser.Guid = settingContainer.GetGuid().ToString();
                            mobUser.MaxDistance = settingContainer.GetMaxDistance();

                            mobEntity = mobUser;
                        }
                    }
                    else
                    {
                        mobEntity = null;
                    }
                }
                else
                {
                    mobEntity = converterService.Convert((item as Entity));
                }

                if (mobEntity != null)
                {
                    this.sqliteConnection.RunInTransaction(() =>
                    {
                        this.sqliteConnection.InsertOrReplaceWithChildren(mobEntity, true);
                    });
                }
            }
            catch(Exception e)
            {
                return;
            }
        }

        public void AddItems<T>(IEnumerable<T> items)
        {
            List<MobileEntity> convertedItems = new List<MobileEntity>();

            foreach (T item in items)
            {
                try
                {
                    MobileEntity mobEntity = converterService.Convert((item as Entity));
                    convertedItems.Add(mobEntity);
                }
                catch (Exception)
                {
                    //ToDo trace
                    continue;
                }
            }

            //http://stackoverflow.com/questions/36774326/poor-performance-sqlite-net-extensions
            this.sqliteConnection.RunInTransaction(() =>
            {
                this.sqliteConnection.InsertOrReplaceAllWithChildren(convertedItems, true);
            });
        }

        public T[] GetItems<T>() where T : class
        {
            Type requestType = typeof(T);
            List<T> items = new List<T>();
            List<MobileEntity> mobEntities = new List<MobileEntity>();


            if (requestType == typeof(Event))
            {
                List<MobileEvent> mobileItems = this.sqliteConnection.GetAllWithChildren<MobileEvent>(null, true);
                if (mobileItems != null)
                {
                    foreach (var mobileItem in mobileItems)
                    {
                        Entity entity = converterService.Convert((mobileItem as MobileEntity));
                        items.Add(entity as T);
                    }
                }
            }
            else if (requestType == typeof (SettingsContainer))
            {
                List<MobileUser> mobileUsers = this.sqliteConnection.GetAllWithChildren<MobileUser>(null, true);

                if (mobileUsers != null && mobileUsers.Count > 0)
                {
                    if (mobileUsers.Count > 1)
                    {
                        throw new InvalidOperationException("More than one user in local database");
                    }

                    //ToDo Use AutoMapper

                    Setting setting = new Setting();

                    int tmpGuid;
                    if (int.TryParse(mobileUsers[0].Guid, out tmpGuid))
                    {
                        setting.Id = tmpGuid;
                        setting.MaxDistance = mobileUsers[0].MaxDistance;
                        setting.EventDateRange = Models.SettingsContainer.Convert(mobileUsers[0].EventDateRange);
                    }
                    //setting.ExcludedCategories = mobileUsers[0].ExcludedCategories;

                    SettingsContainer settingContainer = new SettingsContainer(null,null);
                    settingContainer.SetSetting(setting);


                   items.Add(settingContainer as T);
                }
            }
            else
            {
                throw new InvalidOperationException();
            }

            return items.ToArray();
        }
    }
}
