using System;
using MonkeyCode.Framework.Portable.Urvent.Services;

namespace MonkeyCode.Apps.Urvent.Portable.Models
{
    public interface ISettingsContainer
    {
        double GetMaxDistance();
        void SetMaxDistance(double maxDistance);

        int GetEventDateRange();
        void SetEventDateRange(int eventDateRange);

        Tuple<double,double> GetGeolocation();
        void SetGelocation(double latitude, double longitude);

        event EventHandler<LocationChangedEventArgs> LocationChanged;
    }
}
