using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonkeyCode.Framework.Portable.Urvent.Helper;
using MonkeyCode.Framework.Portable.Urvent.Models;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace MonkeyCode.Framework.Portable.Urvent.Services.Core
{
    public class EventbriteService : HttpDataService, IEventService
    {
        //public IEnumerable<Event> GetEvents()
        //{
        //    return this.GetEvents("stuttgart");
        //}

        //public IEnumerable<Event> GetEvents(string city)
        //{
        //    var result = this.Get($"events/search/?venue.city={city}&expand=venue,category",
        //        new EventbriteJsonReader());
        //    return result;
        //}


        protected override string HttpAddress => "https://www.eventbriteapi.com/v3/";
        //  Wanne's anonymous token
        protected override KeyValuePair<string, string> AuthenticationHeader => 
            new KeyValuePair<string,string>("Bearer", "GZW3GIZUGH7U7GEK2QQW");

        //public IEnumerable<Event> GetEvents(double latitude, double longitude, EventDateRange dateRange)
        //{
        //    var distance = "3km";

        //    //  TODO: ADD DATE RANGE.
        //    var events = this.Get($"events/search/?" +
        //                          $"location.latitude={latitude.InvariantString()}&" +
        //                          $"location.longitude={longitude.InvariantString()}&" +
        //                          $"location.within={distance}&" +
        //                          $"expand=venue,category", new EventbriteJsonReader());
        //    return events;
        //}

        public Task<ImageSource> GetCover(string url, ThumbnailSize size)
        {
            throw new System.NotImplementedException();
        }

        Task<IEnumerable<Event>> IEventService.GetEvents(double latitude, double longitude, EventDateRange dateRange)
        {
            throw new System.NotImplementedException();
        }
    }

    public class EventbriteJsonReader : IJsonReader<Event[]>
    {
        public Event[] ReadJson(string json)
        {
            return this.ReadJsonDeferred(json).ToArray();
        }

        public IEnumerable<Event> ReadJsonDeferred(string json)
        {
            var jsonObject = JObject.Parse(json);
            var eventArray = jsonObject.SelectToken("events");

            foreach (var eventToken in eventArray.Children())
            {
                const string dateTimeFormat = "MM/dd/yyyy HH:mm:ss";

                var customEvent = new Event
                {
                    Id = eventToken.TokenString("id"),
                    Name = eventToken.TokenString("name.text"),
                    Description = eventToken.TokenString("description.text"),
                    StartDateTime = eventToken.TokenDateTime("start.local", dateTimeFormat),
                    EndDateTime = eventToken.TokenDateTime("end.local", dateTimeFormat),
                    Location = new Location
                    {
                        Latitude = eventToken.TokenValue<float>("venue.address.latitude"),
                        Longitude = eventToken.TokenValue<float>("venue.address.longitude"),
                        City = eventToken.TokenString("venue.address.city"),
                        ZipCode = eventToken.TokenString("venue.address.postal_code")
                    },
                    Cover = new Cover
                    {
                        Url = eventToken.TokenString("logo.url"),
                        //  TODO: CREATE THUMBNAILS
                    },
                    Categories = new List<Category>
                    {
                        new Category
                        {
                            Name = eventToken.TokenString("category.name")
                        }
                    },
                    EventType = new EventType
                    {
                        Type = "Eventbrite"
                    }
                };

                yield return customEvent;
            }
        }
    }
}