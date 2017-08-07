using System;
using System.Linq;
using MonkeyCode.Framework.Portable.Urvent.Models;
using MonkeyCode.Framework.Portable.Urvent.Services;

namespace MonkeyCode.Apps.Urvent.Portable.Models
{
    public class SettingsContainer : ISettingsContainer, IDisposable
    {
        private static readonly EventDateRange EventDateRangeToday = new EventDateRange
        {
            Id = 0,
            Description = "Today"
        };

        private static readonly EventDateRange EventDateRangeWeek = new EventDateRange
        {
            Id = 1,
            Description = "Week"
        };

        private static readonly EventDateRange EventDateRangeWeekend = new EventDateRange
        {
            Id = 2,
            Description = "Weekend"
        };

        private static readonly EventDateRange EventDateRangeAll = new EventDateRange
        {
            Id = 3,
            Description = "All"
        };

        private readonly IGeolocationService _geoService;
        private readonly IDataService _dataService;


        private double _latitude;
        private double _longitude;

        private Setting _setting;


        private static SettingsContainer _instance;

        public event EventHandler<LocationChangedEventArgs> LocationChanged;


        public SettingsContainer(IGeolocationService geoservice, IDataService dataService)
        {
            lock (this)
            {
                if (dataService != null)
                {
                    this._dataService = dataService;
                }

                if (geoservice != null)
                {
                    this._geoService = geoservice;
                    this._geoService.LocationChanged += this.OnLocationChanged;
                }
                

                if (_instance != null)
                {
                    throw new InvalidOperationException("SettingsContainer is initialised by AutoFac. Do not create an instance manually");
                }


                //create default settings
                this._setting = new Setting
                {
                    MaxDistance = 20,
                    ExcludedCategories = null,
                    Id = Guid.NewGuid().ToString().GetHashCode(),
                    EventDateRange = EventDateRangeWeek
                };

                var savedSettings = this._dataService?.GetItems<SettingsContainer>();
                if (savedSettings != null && savedSettings.Any())
                {
                    //load database settings
                    this._setting = savedSettings[0]._setting;
                    this._latitude = savedSettings[0]._latitude;
                    this._longitude = savedSettings[0]._longitude;

                    //SetEventDateRange(savedSettings[0].GetEventDateRange());
                    //SetMaxDistance(savedSettings[0].GetMaxDistance());
                }


                _instance = this;
            }
        }

        public virtual void Dispose()
        {
            this._dataService?.AddItem(this);
        }

        private void OnLocationChanged(object sender, LocationChangedEventArgs e)
        {
            if (e.IsGeolocationEnabled)
            {
                this.SetGelocation(e.Latitude, e.Longitude);
                this.LocationChanged?.Invoke(this,e);
            }
        }


        public double GetMaxDistance()
        { 
            lock(this)
            { 
                return this._setting.MaxDistance;
            }
        }

        public void SetMaxDistance(double maxDistance)
        {
            lock(this)
            {
                this._setting.MaxDistance = maxDistance;

                this._dataService?.AddItem(this);
            }
        }

        public int GetGuid()
        {
            lock (this)
            {
                return this._setting.Id;
            }
        }

        /// <summary>
        /// latitude, longitude
        /// </summary>
        /// <returns></returns>
        public Tuple<double, double> GetGeolocation()
        {
            lock (this)
            {
                return new Tuple<double, double>(this._latitude, this._longitude);
            }
        }

        public void SetGelocation(double latitude, double longitude)
        {
            lock (this)
            {
                this._latitude = latitude;
                this._longitude = longitude;
            }
        }

        public void SetSetting(Setting setting)
        {
            lock (this)
            {
                this._setting = setting;
            }
        }

        public int GetEventDateRange()
        {
            lock(this)
            { 
                return this._setting.EventDateRange.Id;
            }
        }

        public static EventDateRange Convert (int eventDateRange)
        {
            switch (eventDateRange)
            {
                case 0:
                    return EventDateRangeToday;
                case 1:
                    return EventDateRangeWeek;
                case 2:
                    return EventDateRangeWeekend;
                case 3:
                    return EventDateRangeAll;
                default:
                    return null;
            }
        }


        public void SetEventDateRange(int eventDateRange)
        {
            lock (this)
            {
                this._setting.EventDateRange = Convert(eventDateRange);
                this._dataService?.AddItem(this);
            }
        }
    }
}
