using System.Collections.Generic;
using System.Linq;
using MonkeyCode.Framework.Portable.Urvent.Services;
using MonkeyCode.Framework.Portable.Urvent.Services.Core;
using MonkeyCode.Framework.Tests.Urvent.Core;
using NUnit.Framework;

namespace MonkeyCode.Framework.Tests.Urvent.BusinessLogicTests
{
    [TestFixture]
    internal class EventServiceTests
    {

        public static IEnumerable<IEventService> EventServicesSource
        {
            get
            {
                yield return new FacebookService();
                yield return new EventbriteService();
                yield return new MeetUpService();
                yield return new MockEventService();
                yield return new UrventService();
            }
        }

        //private static Func<T> Func<T>(Func<T> get) => get;

        [TestCaseSource(nameof(EventServicesSource))]
        public void GetEvents(IEventService getService)
        {
            var service = getService;
            var location = Cities.München.GetLocation();
            var events = service.GetEvents(location.Item1, location.Item2, DateRanges.Week.ToEventDateRange()).Result.ToArray();

            Assert.IsTrue(events.Any(), "No events received.");
            Assert.IsTrue(events.All(e => !string.IsNullOrWhiteSpace(e.Name)), "Not all events have a name.");
        }
    }
}
