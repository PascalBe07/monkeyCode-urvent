using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MonkeyCode.Framework.Portable.Urvent.Models;
using MonkeyCode.Framework.Portable.Urvent.Models.Mobile;
using MonkeyCode.Framework.Portable.Urvent.Services;
using MonkeyCode.Framework.Portable.Urvent.Services.Core;
using MonkeyCode.Framework.Tests.Urvent.Core;
using NUnit.Framework;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLiteNetExtensions.Extensions;


namespace MonkeyCode.Framework.Tests.Urvent.AutoMapperTests
{
    [TestFixture]
    public class AutoMapperTests
    {
        private IModelConverterService _converterService;
        private Event[] _events;
        private SQLiteConnection _connection;
        private MobileUser _dummyUser;

        [SetUp]
        public void SetUp()
        {
            this._converterService = new ModelConverterService();
            IEventService eventService = new EventbriteService();
            var location = Cities.Amberg.GetLocation();
            this._events = 
                eventService.GetEvents(location.Item1, location.Item2, new EventDateRange {Description = "Weekend"}).Result
                    .ToArray();


            var fileName = "SQliteDatabaseTest.db3";
            var path = Path.Combine(Path.GetTempPath(), fileName);

            Console.WriteLine(path);
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            this._connection = new SQLiteConnection(new SQLite.Net.Platform.Win32.SQLitePlatformWin32(), path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
            this._connection.CreateTable<MobileCategory>();
            this._connection.CreateTable<MobileUser>();
            this._connection.CreateTable<MobileEvent>();


            this._dummyUser = new MobileUser
            {
                Guid = "max.urvent",
                Gender = 0,
                EMail = "max@urvent.com",
                Birthday = DateTime.Now,
                AccessToken = "token",
                AccessTokenReceived = DateTime.Now,
                AttractedTo = 1,
                EventDateRange = 0,
                ExcludedCategories = new List<MobileCategory>(),
                FacebookAccessToken = "fbToken",
                FacebookAccessTokenReceived = DateTime.Now,
                MaxDistance = 20,
                Longitude = 0,
                Latitude = 0,
                LastLogin = DateTime.Now,
                UserEvents = new List<MobileEvent>()
            };

        }

        [TestCase]
        public void EventToMobileEvent()
        {
            Assert.IsTrue(this._events.Any(), "No events received.");


            foreach (var eventToConvert in this._events)
            {
                var mobileEntity = this._converterService.Convert((Entity)eventToConvert);

                var mobileEvent = mobileEntity as MobileEvent;

                if (mobileEvent != null)
                {
                    Assert.AreEqual(eventToConvert.Id, mobileEvent.Id);
                    Assert.AreEqual(eventToConvert.Cover.Url, mobileEvent.CoverUrl);

                    /*
                 * Thumbnail creation not implemented yet
                Assert.AreEqual(eventToConvert.Cover.ThumbnailSmall, mobileEvent.ThumbnailSmall);
                Assert.AreEqual(eventToConvert.Cover.ThumbnailMedium, mobileEvent.ThumbnailMedium);
                Assert.AreEqual(eventToConvert.Cover.ThumbnailLarge, mobileEvent.ThumbnailLarge);
                */

                    Assert.AreEqual(eventToConvert.Location.Longitude, mobileEvent.Longitude);
                    Assert.AreEqual(eventToConvert.Location.Latitude, mobileEvent.Latitude);

                    foreach (var category in mobileEvent.Categories)
                    {
                        Assert.AreEqual(category.EventId, mobileEvent.Id);
                    }

                    if (!this._dummyUser.UserEvents.Contains(mobileEvent))
                    {
                        this._dummyUser.UserEvents.Add(mobileEvent);
                    }
                }
            }

            try
            {
                this._connection.InsertWithChildren(this._dummyUser, true);
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message);
            }
           
        }


        /*
        [TestCase]
        public void MobileCategoryToBackendCategoryMapperTest()
        {
            MobileCategory mobileCategory = new MobileCategory()
            {
                Id = 1,
                Name = "MobileCateogoryTest",

                EventId = "MobileEventIdTest",
                UserGuid = "MobileUserGuidTest"
            };
   
            Category backendCategory= autoMapper.Map<Category>(mobileCategory);

            Assert.AreEqual(backendCategory.Id,mobileCategory.Id);
            Assert.AreEqual(backendCategory.Name, mobileCategory.Name);
        }

        [TestCase]
        public void MobileCategoryToBackendUserMapperTest()
        {
            MobileCategory mobileCategory = new MobileCategory()
            {
                Id = 1,
                Name = "MobileCateogoryTest",

                EventId = "MobileEventIdTest",
                UserGuid = "MobileUserGuidTest"
            };

            User backendUser = autoMapper.Map<User>(mobileCategory);
            Assert.AreEqual(backendUser.UrventUserData.Guid, mobileCategory.UserGuid);
            //ToDo add settings
        }


        [TestCase]
        public void UserCatagoryToMobileCategoryMapperTest()
        {
            Category backendCategory = new Category()
            {
                Id = 1,
                Name = "CategoryTest"
            };


            UrventUserData backendUrventUserData = new UrventUserData()
            {
                Guid = "TestGuid",
                AccessToken = "TestAccessToken",
                AccessTokenReceived = DateTime.Now
            };

            User backendUser = new User()
            {
                EMail = "MailTest",
                UrventUserData = backendUrventUserData
            };

            MobileCategory mobileCategory = new MobileCategory();
            autoMapper.Map(backendCategory, mobileCategory);
            autoMapper.Map(backendUser, mobileCategory);

            Assert.AreEqual(backendCategory.Id,mobileCategory.Id);
            Assert.AreEqual(backendCategory.Name, mobileCategory.Name);
            Assert.AreEqual(mobileCategory.UserGuid, backendUser.UrventUserData.Guid);

            Assert.IsNull(mobileCategory.EventId);           
        }

        [TestCase]
        public void EventCategoryToMobileCategoryMapperTest()
        {
            Category backendCategory = new Category()
            {
                Id = 1,
                Name = "CategoryTest"
            };

            Category backendCategory2 = new Category()
            {
                Id = 2,
                Name = "CategoryTest2"
            };


            Event backendUrventUserData = new Event()
            {
                Id = "1",
                EventTypeId = 1,
                Categories = new List<Category>()
            };

            backendUrventUserData.Categories.Add(backendCategory);
            backendUrventUserData.Categories.Add(backendCategory2);

            
            MobileCategory mobileCategory = new MobileCategory();
            autoMapper.Map(backendUrventUserData, mobileCategory);

            Assert.AreEqual(backendUrventUserData.Id, mobileCategory.EventId);

            
            Assert.AreEqual(backendCategory.Id, mobileCategory.Id);
            Assert.AreEqual(backendCategory.Name, mobileCategory.Name);
            Assert.IsNull(mobileCategory.EventId);
            
        }
        */

    }
}
