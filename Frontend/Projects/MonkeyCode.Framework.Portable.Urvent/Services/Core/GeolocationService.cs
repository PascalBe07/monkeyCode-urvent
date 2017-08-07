using System;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace MonkeyCode.Framework.Portable.Urvent.Services.Core
{
    public class GeolocationService : IGeolocationService
    {
        private const int MinUpdateTimeMilliseconds = 1000;
        private const int MinDistanceMeter = 1;

        private IGeolocator _locator;

        public event EventHandler<LocationChangedEventArgs> LocationChanged;

        public GeolocationService()
        {
            this._locator = CrossGeolocator.Current;
            //_locator.AllowsBackgroundUpdates = true;
            this._locator.DesiredAccuracy = 50;

            this._locator.PositionChanged += this.OnGeolocatorPositionChanged;

            this._locator.StartListeningAsync(2500, 10.0f, true);
        }

        ~GeolocationService()
        {
            this._locator.StopListeningAsync();
        }

        void OnGeolocatorPositionChanged(object sender, PositionEventArgs e)
        {
            this.OnLocationChanged(this, new LocationChangedEventArgs(e.Position.Latitude, e.Position.Longitude, this.IsGeolocationOn()));
        }


        protected virtual void OnLocationChanged(object sender, LocationChangedEventArgs e)
        {
            if (this.LocationChanged != null)
            {
                this.LocationChanged(this, e);
            }
        }

        public async Task<Tuple<double, double>> GetLocation()
        {
            try
            {
                if (!this._locator.IsGeolocationAvailable)
                    throw new NotSupportedException("Geolocation not available");
                if (!this._locator.IsGeolocationEnabled)
                    throw new GeolocationException(GeolocationError.PositionUnavailable);

                Position position = await this._locator.GetPositionAsync(timeoutMilliseconds: 10000);
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
            this._locator.DesiredAccuracy = accurancy;
        }

        public double GetAccurancy()
        {
            return this._locator.DesiredAccuracy;
        }

        public bool IsGeolocationOn()
        {
            return this._locator.IsGeolocationAvailable && this._locator.IsGeolocationEnabled;
        }
    }
}
