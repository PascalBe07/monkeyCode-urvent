using System.Collections.Generic;
using System.Threading.Tasks;
using MonkeyCode.Framework.Portable.Urvent.Models;
using Xamarin.Forms;

namespace MonkeyCode.Framework.Portable.Urvent.Services
{
    public interface IEventService
    {
        //IEnumerable<Event> GetEvents();

        Task<IEnumerable<Event>> GetEvents(double latitude, double longitude, EventDateRange dateRange);

        Task<ImageSource> GetCover(string url, ThumbnailSize size);

        //IEnumerable<Event> GetEvents(string city);
    }
}
