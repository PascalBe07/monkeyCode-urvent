using MonkeyCode.Apps.Urvent.Portable.Models;
using MonkeyCode.Framework.Portable.Urvent.Services;
using Xamarin.Forms;


namespace MonkeyCode.Apps.Urvent.Portable.ViewModels
{
    public class SettingsViewModel : Framework.Portable.ViewModel.ViewModel
    {
        private readonly ISettingsContainer _settingsContainer;

        public SettingsViewModel(ISettingsContainer settingsContainer)
        {
            this.Latitude = "undefined";
            this.Longitude = "undefined";

            this._settingsContainer = settingsContainer;
            this._settingsContainer.LocationChanged += this.OnLocationChanged;

            this.Title = "Settings";
        }

        private void OnLocationChanged(object sender, LocationChangedEventArgs e)
        {
            this.Latitude = $"{e.Latitude:F4}";
            this.Longitude = $"{e.Longitude:F4}";
            this.OnPropertyChanged();
        }


        //  NOT USED
        //public void OnMaxDistanceChanged(object sender, ValueChangedEventArgs e)
        //{
        //    this._settingsContainer.SetMaxDistance(e.NewValue);
        //}

        //  INTRODUCED DOUBLE VARIABLE MAX DISTANCE
        //public string MaxDistance
        //{
        //    //ToDo: set constant parts in xaml
        //    get { return "Max. distance " + this._settingsContainer.GetMaxDistance() + " km"; }
        //    set
        //    {
        //        double rangeValue;
        //        if (double.TryParse(value, out rangeValue))
        //        {
        //            this._settingsContainer.SetMaxDistance(rangeValue);
        //            this.OnPropertyChanged();
        //        }
        //    }
        //}

        public double MaxDistance
        {
            get
            {
                return this._settingsContainer.GetMaxDistance();
            }
            set
            {
                this._settingsContainer.SetMaxDistance(value);
                this.OnPropertyChanged();
            }
        }

        public int EventDateRange
        {
            get { return this._settingsContainer.GetEventDateRange(); }
            set
            {
                this._settingsContainer.SetEventDateRange(value);
                this.OnPropertyChanged();
            }
        }


        // ===================================================================================================
        // JUST FOR DEVELOPMENT

        
        public string Latitude
        {
            get { return this._settingsContainer.GetGeolocation().Item1.ToString(); }
            set
            {
                double tmpLatitude;
                if (double.TryParse(value, out tmpLatitude))
                {
                    this._settingsContainer.SetGelocation(tmpLatitude, this._settingsContainer.GetGeolocation().Item2);
                    this.OnPropertyChanged();
                }
            }
        }


        public string Longitude
        {
            get { return this._settingsContainer.GetGeolocation().Item2.ToString(); }
            set
            {
                double tmpLatitude;
                if (double.TryParse(value, out tmpLatitude))
                {
                    this._settingsContainer.SetGelocation(this._settingsContainer.GetGeolocation().Item1, tmpLatitude);
                    this.OnPropertyChanged();
                }
            }
        }

        // ===================================================================================================


    }
}
