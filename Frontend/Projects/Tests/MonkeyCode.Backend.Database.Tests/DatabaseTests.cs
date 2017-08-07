
using System;
using MonkeyCode.Framework.Portable.Urvent.Models;
using NUnit.Framework;

namespace MonkeyCode.Backend.Database.Tests
{
    [TestFixture]
    public class DatabaseTest
    {

        [TestCase]
        public void ConnectDatabase()
        {
            var ctx = new UrventDatabaseContext();
            
            Category catParty = new Category()
            {
                Name = "Party"
            };
            Category catSport = new Category()
            {
                Name = "Sport"
            };
            ctx.Category.Add(catParty);
            ctx.Category.Add(catSport);

            Gender male = new Gender()
            {
                Type = "Male"
            };
            Gender female = new Gender()
            {
                Type = "Female"
            };
            ctx.Gender.Add(male);
            ctx.Gender.Add(female);

            EventType fbEventType = new EventType()
            {
                Type ="Facebook"
            };
            EventType eventbriteEventType = new EventType()
            {
                Type = "EventBrite"
            };
            ctx.EventType.Add(fbEventType);
            ctx.EventType.Add(eventbriteEventType);

            Event dum = new Event()
            {
                Name="Event_1",
                AttendingCount = 0,
                AttendingCountFemale = 0,
                AttendingCountMale = 0,
                Description = "Test",
                Id ="1",
                EventType = fbEventType
            };
            dum.Categories.Add(catParty);


            ctx.Event.Add(dum);

            try
            {
                ctx.SaveChanges();
            }
            catch (Exception ec)
            {
                Console.WriteLine(ec.InnerException.ToString());
                Assert.IsFalse(true);
            }
        }
    }
}
