using System;
using System.Threading.Tasks;

namespace MonkeyCode.Framework.Portable.Urvent.Services
{
    public class LocationChangedEventArgs : EventArgs
    {
        public LocationChangedEventArgs(double longitude, double latitude, bool geoLocationOn)
        {
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.IsGeolocationEnabled = geoLocationOn;
        }

        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public bool IsGeolocationEnabled { get; set; }
    }

    public interface IGeolocationService
    {
        Task<Tuple<double, double>> GetLocation();

        void SetAccurancy(double accurancy);
        double GetAccurancy();

        event EventHandler<LocationChangedEventArgs> LocationChanged;

        bool IsGeolocationOn();
    }
}
