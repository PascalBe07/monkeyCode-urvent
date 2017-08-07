using System;
using System.Threading.Tasks;

namespace MonkeyCode.Framework.Portable.Urvent.Services.Core
{
    public class MockGeoLocationService : IGeolocationService
    {
        public Task<Tuple<double, double>> GetLocation()
        {
            return new Task<Tuple<double, double>>(() => new Tuple<double, double>(49.02, 12.08));
        }

        public void SetAccurancy(double accurancy)
        {
            //  MOCK
            return;
        }

        public double GetAccurancy()
        {
            //  MOCK
            return 1;
        }

        public event EventHandler<LocationChangedEventArgs> LocationChanged;
        public bool IsGeolocationOn()
        {
            //  MOCK
            return true;
        }
    }
}