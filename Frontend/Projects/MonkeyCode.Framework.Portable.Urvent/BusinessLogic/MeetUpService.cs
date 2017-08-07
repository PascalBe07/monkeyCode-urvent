using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonkeyCode.Framework.Portable.Urvent.Models;
using Newtonsoft.Json.Linq;

namespace MonkeyCode.Framework.Portable.Urvent.BusinessLogic
{
    public class MeetUpService : JsonHttpService, IEventService
    {
        protected override string HttpAddress => @"https://api.meetup.com/2/";
        //protected override KeyValuePair<string,string> AuthenticationHeader 
        //    => new KeyValuePair<string, string>("Bearer", "CURRENTLY REMOVED");

        //public IEnumerable<Event> GetEvents()
        //{
        //    //  TODO: PUT AUTHENTICATION IN HTTP HEADER
        //    var events = this.Get(
        //        $"open_events?&sign=true&photo-host=secure&lat=52.523405&lon=13.4114&" +
        //        $"fields=group_photo,photo_sample,category&key=66164c2458624c3374312f29225201a", 
        //        new MeetUpJsonReader());
        //    return events;
        //}

        //public IEnumerable<Event> GetEvents(string city)
        //{
        //    return this.GetEvents();
        //}
        public IEnumerable<Event> GetEvents(double latitude, double longitude, EventDateRange dateRange)
        {
            //  TODO: PUT AUTHENTICATION IN HTTP HEADER
            //  TODO: ADD DATE RANGE.
            var events = this.Get(
                $"open_events?&sign=true&photo-host=secure&" +
                $"lat={latitude.InvariantString()}&lon={longitude.InvariantString()}&" +
                $"fields=group_photo,photo_sample,category&key=66164c2458624c3374312f29225201a",
                new MeetUpJsonReader());
            return events;
        }

        public Task<Cover> GetCover(string url, ThumbnailSize size)
        {
            throw new NotImplementedException();
        }
    }

    public class MeetUpJsonReader : IJsonReader<Event[]>
    {
        public Event[] ReadJson(string json)
        {
            return this.ReadJsonDeferred(json).ToArray();
        }

        public IEnumerable<Event> ReadJsonDeferred(string json)
        {
            var jsonObject = JObject.Parse(json);
            var eventArray = jsonObject.SelectToken("results");

            foreach (var eventToken in eventArray.Children())
            {
                //  TODO: FIX POSSIBLE ERROR IF NO TIME OR DURATION VALUE IS SET.
                var startDateTime = new DateTime(1970, 1, 1).AddMilliseconds(eventToken.TokenValue<long>("time"));
                var endDateTime = startDateTime.AddMilliseconds(eventToken.TokenValue("duration",
                    Convert.ToInt64(3.Hours().TotalMilliseconds)));
                
                var customEvent = new Event
                {
                    Id = eventToken.TokenString("id"),
                    Name = eventToken.TokenString("name"),
                    Description = eventToken.TokenString("description"),
                    StartDateTime = startDateTime,
                    EndDateTime = endDateTime,
                    AttendingCount = eventToken.TokenValue<int>("yes_rsvp_count"),
                    Location = new Location
                    {
                        Latitude = eventToken.TokenValue<float>("venue.lat"),
                        Longitude = eventToken.TokenValue<float>("venue.lon"),
                        City = eventToken.TokenString("venue.city"),
                        Street = eventToken.TokenString("venue.address_1"),
                    },
                    EventType = new EventType
                    {
                        Type = "MeetUp"
                    },
                };

                yield return customEvent;
            }
        }
    }
}