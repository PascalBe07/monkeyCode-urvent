using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using MonkeyCode.Framework.Portable.Urvent.Models;

namespace MonkeyCode.Framework.Portable.Urvent.BusinessLogic
{
    public class UrventService : JsonHttpService, IEventService
    {
        //public IEnumerable<Event> GetEvents()
        //{
        //    return this.GetEvents("Stuttgart");
        //}
        

        public IEnumerable<Event> GetEvents(double latitude, double longitude, EventDateRange dateRange)
        {
            var events = this.Get<Event[]>($"get/events/{dateRange.Description}?" +
                                           $"lat={latitude.InvariantString()}&" +
                                           $"long={longitude.InvariantString()}");
            return events;
        }

        public async Task<Cover> GetCover(string url, ThumbnailSize size)
        {
            var retVal = new Cover()
            {
                Url =  url
            };

            using (var client = new HttpClient())
            {
                byte[] res;
                try
                {
                    var combinedUrl = this.HttpAddress +
                                      $"get/thumbnail?Size={Enum.GetName(typeof(ThumbnailSize), size)}&Url={url}";
                    res =  await client.GetByteArrayAsync(new Uri(combinedUrl));
                }
                catch (Exception ex)
                {
                    return null;
                }

                switch (size)
                {
                    case ThumbnailSize.Large:
                        retVal.ThumbnailLarge = res;
                        break;
                        case ThumbnailSize.Medium:
                        retVal.ThumbnailMedium = res;
                        break;
                        case ThumbnailSize.Small:
                        retVal.ThumbnailSmall = res;
                        break;
                    default:
                        break;
                }
                return retVal;
            }
        }


        protected override string HttpAddress => "http://h2622931.stratoserver.net:3000/api/";
    }
}