using System;
using System.Collections.ObjectModel;
using MonkeyCode.Framework.Portable.Urvent.Models;
using MonkeyCode.Framework.Portable.Urvent.Services;
using MonkeyCode.Framework.Portable.ViewModel;

namespace MonkeyCode.Apps.Urvent.Forms.ViewModels
{
    public class EventOverviewViewModel : ViewModel
    {
        private readonly IEventService _eventService;
        private readonly IDataService _dataService;
        private readonly IGeolocationService _locationService;

        public EventOverviewViewModel(IEventService eventService, IDataService dataService, 
            IGeolocationService locationService)
        {
            this.Title = "Near events";
            this._eventService = eventService;
            this._dataService = dataService;
            this._locationService = locationService;

            this.LoadEvents();
        }


        private ObservableCollection<Event> _events = new ObservableCollection<Event>();
        public ObservableCollection<Event> Events
        {
            get { return this._events; }
            set { this._events = value; this.OnPropertyChanged(); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return this._isLoading; }
            set { this._isLoading = value; this.OnPropertyChanged(); }
        }

        private bool _hasEvents;
        public bool HasEvents
        {
            get { return this._hasEvents; }
            set
            {
                this._hasEvents = value;
                this.OnPropertyChanged();
            }
        }

        private string _message;

        public string Message
        {
            get { return this._message; }
            set
            {
                this._message = value;
                this.OnPropertyChanged();
            }
        }

        private async void LoadEvents()
        {
            this.IsLoading = true;
            this.Message = "Load events started ...";
            var currentLocation = await this._locationService.GetLocation();
            var events = await this._eventService.GetEvents(currentLocation.Item1, currentLocation.Item2,
                new EventDateRange {Description = "Week"});
            this.Events = new ObservableCollection<Event>(events);
            this.Message = "Load events finished ...";
            this.IsLoading = false;
        }

        private void OnError(Exception ex)
        {
            this.Message = "Error: " + ex.Message;
            //  TODO: MESSAGE BOX
        }
    }
}
