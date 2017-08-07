using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MonkeyCode.Framework.Portable.Urvent.Helper;
using MonkeyCode.Framework.Portable.Urvent.Models;
using Xamarin.Forms;

namespace MonkeyCode.Framework.Portable.Urvent.Services.Core
{
    public class UrventService : HttpDataService, IEventService
    {
        //public IEnumerable<Event> GetEvents()
        //{
        //    return this.GetEvents("Stuttgart");
        //}
        

        public async Task<IEnumerable<Event>> GetEvents(double latitude, double longitude, EventDateRange dateRange)
        {
            var corr_lat = latitude.InvariantString() == "0" ? "48.8" : latitude.InvariantString();
            var corr_long = longitude.InvariantString() == "0" ? "11.3" : longitude.InvariantString();

            var events = await this.Get<Event[]>($"get/events/{dateRange.Description}?" +
                                           $"lat={corr_lat}&" +
                                           $"long={corr_long}");
            return events;
        }

        public async Task<ImageSource> GetCover(string url, ThumbnailSize size)
        {
            var bytes = await this.GetBytes($"get/thumbnail?Size={Enum.GetName(typeof(ThumbnailSize), size)}&Url={url}");
            return ImageSource.FromStream(() => new MemoryStream(bytes));
        }

        protected override string HttpAddress => "http://85.214.229.66:3000/api/";
    }
}