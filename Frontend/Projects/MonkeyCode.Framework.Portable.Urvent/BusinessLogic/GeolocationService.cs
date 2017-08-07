using System;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Threading.Tasks;
using MonkeyCode.Framework.Portable.Urvent.Models;

namespace MonkeyCode.Framework.Portable.Urvent.BusinessLogic
{
    public class GeolocationService : IGeolocationService
    {
        private const int MinUpdateTimeMilliseconds = 1000;
        private const int MinDistanceMeter = 1;

        private IGeolocator _locator;

        public event EventHandler<LocationChangedEventArgs> LocationChanged;

        public GeolocationService()
        {
            _locator = CrossGeolocator.Current;
            //_locator.AllowsBackgroundUpdates = true;
            _locator.DesiredAccuracy = 50;

            _locator.PositionChanged += OnGeolocatorPositionChanged;

            _locator.StartListeningAsync(2500, 10.0f, true);
        }

        ~GeolocationService()
        {
            _locator.StopListeningAsync();
        }

        void OnGeolocatorPositionChanged(object sender, PositionEventArgs e)
        {
            OnLocationChanged(this, new LocationChangedEventArgs(e.Position.Latitude, e.Position.Longitude, IsGeolocationOn()));
        }


        protected virtual void OnLocationChanged(object sender, LocationChangedEventArgs e)
        {
            if (LocationChanged != null)
            {
                LocationChanged(this, e);
            }
        }

        public async Task<Tuple<double, double>> GetLocation()
        {
            try
            {
                if (!_locator.IsGeolocationAvailable)
                    throw new NotSupportedException("Geolocation not available");
                if (!_locator.IsGeolocationEnabled)
                    throw new GeolocationException(GeolocationError.PositionUnavailable);

                Position position = await _locator.GetPositionAsync(timeoutMilliseconds: 10000);
                return new Tuple<double, double>(position.Latitude, position.Longitude);
            }
            catch (Exception ex)
            {
                //TODO: Add error logging
                return null;
            }
        }

        public void SetAccurancy(double accurancy)
        {
            _locator.DesiredAccuracy = accurancy;
        }

        public double GetAccurancy()
        {
            return _locator.DesiredAccuracy;
        }

        public bool IsGeolocationOn()
        {
            return _locator.IsGeolocationAvailable && _locator.IsGeolocationEnabled;
        }
    }
}
