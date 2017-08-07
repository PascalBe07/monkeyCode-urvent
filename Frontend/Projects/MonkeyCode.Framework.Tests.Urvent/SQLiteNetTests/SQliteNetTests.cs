using System;
using System.Collections.Generic;
using System.IO;
using MonkeyCode.Framework.Portable.Urvent.Models.Mobile;
using NUnit.Framework;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLiteNetExtensions.Extensions;

namespace MonkeyCode.Framework.Tests.Urvent.SQLiteNetTests
{
    [TestFixture]
    public class SQliteNetTests
    {
        private SQLiteConnection _connection;

        [TestCase]
        public void ConnectDatabase()
        { 
            var fileName = "SQliteDatabaseTest.db3";
            var path = Path.Combine(Path.GetTempPath(), fileName);


            Console.WriteLine(path);
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            _connection = new SQLiteConnection(new SQLite.Net.Platform.Win32.SQLitePlatformWin32(), path, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
            _connection.CreateTable<MobileCategory>();
            _connection.CreateTable<MobileUser>();
            _connection.CreateTable<MobileEvent>();

            MobileCategory party = new MobileCategory()
            {
                Id = 0,
                Name = "Party"
            };


            MobileCategory sport = new MobileCategory()
            {
                Id = 1,
                Name = "Sport"
            };


            MobileEvent neunzigerParty = new MobileEvent()
            {
                Categories = new List<MobileCategory>(),
                City = "Amberg",
                Description = "90iger Party",
                Id = "0",
                EventType = 0
            };
            neunzigerParty.Categories.Add(party);

            MobileUser max = new MobileUser()
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

            max.UserEvents.Add(neunzigerParty);


            //_connection.Insert(party);
            //_connection.Insert(sport);
            //_connection.Insert(neunzigerParty);
            _connection.InsertWithChildren(max,true);
        }

    }
}
