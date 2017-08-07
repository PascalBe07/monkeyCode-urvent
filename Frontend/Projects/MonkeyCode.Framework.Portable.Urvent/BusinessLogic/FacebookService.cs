using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonkeyCode.Framework.Portable.Urvent.Models;
using Newtonsoft.Json.Linq;

namespace MonkeyCode.Framework.Portable.Urvent.BusinessLogic
{
    public class FacebookService : JsonHttpService, IEventService
    {
        protected override string HttpAddress => "https://graph.facebook.com/v2.7/";

        protected override KeyValuePair<string, string> AuthenticationHeader => new KeyValuePair<string, string>("Bearer", 
            "CURRENTLY REMOVED");

        //public IEnumerable<Event> GetEvents()
        //{
        //    return this.GetEvents("Stuttgart");
        //}

        //public IEnumerable<Event> GetEvents(string city)
        //{
        //    var result = this.Get($"search?type=event&q={city}", new FacebookJsonReader());
        //    return result;
        //}
        public IEnumerable<Event> GetEvents(double latitude, double longitude, EventDateRange dateRange)
        {
            throw new NotImplementedException();
        }

        public Task<Cover> GetCover(string url, ThumbnailSize size)
        {
            throw new NotImplementedException();
        }
    }

    public class FacebookJsonReader : IJsonReader<Event[]>
    {
        public Event[] ReadJson(string json)
        {
            return this.ReadJsonDeferred(json).ToArray();
        }

        public IEnumerable<Event> ReadJsonDeferred(string json)
        {
            var jsonObject = JObject.Parse(json);
            var eventArray = jsonObject.SelectToken("data");

            foreach (var eventToken in eventArray.Children())
            {
                const string dateTimeFormat = "MM/dd/yyyy HH:mm:ss";

                var customEvent = new Event
                {
                    Id = eventToken.TokenString("id"),
                    Name = eventToken.TokenString("name"),
                    Description = eventToken.TokenString("description"),
                    StartDateTime = eventToken.TokenDateTime("start_time", dateTimeFormat),
                    EndDateTime = eventToken.TokenDateTime("end_time", dateTimeFormat),
                    Location = new Location
                    {
                        Latitude = eventToken.TokenValue<float>("place.location.latitude"),
                        Longitude = eventToken.TokenValue<float>("place.location.longitude"),
                        City = eventToken.TokenString("place.location.city"),
                    },
                    EventType = new EventType
                    {
                        Type = "Facebook"
                    }
                    //  TODO: ADJUST CALL TO GET MORE INFORMATION
                };

                yield return customEvent;
            }
        }
    }
}