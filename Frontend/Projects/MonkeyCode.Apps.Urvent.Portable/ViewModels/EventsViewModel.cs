using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MonkeyCode.Apps.Urvent.Portable.Models;
using MonkeyCode.Apps.Urvent.Portable.Views;
using MonkeyCode.Framework.Portable.Urvent.Models;
using MonkeyCode.Framework.Portable.Urvent.Services;
using MonkeyCode.Framework.Portable.XamarinForms.DependencyInjection;
using Xamarin.Forms;

namespace MonkeyCode.Apps.Urvent.Portable.ViewModels
{
    public class EventsViewModel : Framework.Portable.ViewModel.ViewModel
    {
        private readonly INavigator _navigator;
        private readonly IEventService _service;
        private readonly IDataService _localDataService;
        private readonly ISettingsContainer _settingsContainer;
        private ObservableCollection<Event> _events;
        private Event _currentEvent;
        private EventViewModel _currentEventViewModel;
        private ICommand _yesCommand;
        private ICommand _noCommand;
        private ICommand _selectCommand;
        private bool _hasEvents;
        private bool _infoVisible;
        private string _infoString;

        public EventsViewModel(INavigator navigator, IEventService service, IDataService localDataService, ISettingsContainer settingsContainer)
        {
            this._navigator = navigator;
            this._service = service;
            this._localDataService = localDataService;
            this._settingsContainer = settingsContainer;
            this.LocalEvents = this._localDataService.GetItems<Event>();
            this.Title = "Events";
            this._hasEvents = true;
            this._settingsContainer.LocationChanged += this._settingsContainer_LocationChanged;
        }

        private Event[] LocalEvents { get; }


        public ObservableCollection<Event> Events
        {
            get { return this._events ?? (this._events = new ObservableCollection<Event>()); }
            private set
            {
                this._events = value;
                this.OnPropertyChanged(propertyName: nameof(this.Events));
            }
        }


        private async Task<Event[]> GetEvents()
        {
            try
            {
                //  TODO: BETTER HANDLE OF ERRORS
                var location = this._settingsContainer.GetGeolocation();
                var eventDateRange = this._settingsContainer.GetEventDateRange();
                var events =
                    await
                        this._service.GetEvents(location.Item1, location.Item2,
                            SettingsContainer.Convert(eventDateRange));
                var eventsArray = events.ToArray();
                
                
                //  TODO: BIND BUTTON ENABLING TO COMMANDS
                if (!eventsArray.Any()) this.HasEvents = false;
                return eventsArray;
            }
            catch (Exception ex)
            {
                this.HasEvents = false;
                return new Event[0];
            }
            
        }

        private void _settingsContainer_LocationChanged(object sender, LocationChangedEventArgs e)
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            this.Events = null;
        }


        public EventViewModel CurrentEventVm
        {
            get
            {
                if (_currentEventViewModel == null)
                {
                    //Startup
                    _currentEventViewModel = new EventViewModel() { IsBusy = true, Title = "Loading" };
                    Task.Factory.StartNew(() =>
                    {
                        this.Events = new ObservableCollection<Event>(this.GetEvents().Result.Where(e =>
                            !this.LocalEvents.Any(s => s.Id.Equals(e.Id))));
                        _currentEventViewModel.IsBusy = false;
                        CurrentEvent = this.Events.FirstOrDefault();
                    });
                }
                return _currentEventViewModel;
            }
            private set
            {
                _currentEventViewModel = value;
                OnPropertyChanged(nameof(CurrentEventVm));
            }
        }


        public Event CurrentEvent
        {
            get
            {
                if (this._currentEvent != null) return this._currentEvent;
                if (this._events == null || this._events.Count == 0) return null;

                //At first get UserEventStatus has to be defined before getting the element
                this._currentEvent = this.Events.FirstOrDefault();
                UserEvent userEvent = new UserEvent();
                userEvent.EventId = this._currentEvent.Id;
                userEvent.EventType = this._currentEvent.EventType;
                userEvent.Events = new List<Event>();
                userEvent.Events.Add(this._currentEvent);

                //Todo global constant
                userEvent.Status = new UserEventStatus
                {
                    Id = 0,
                    Status = "unknown"
                };
                userEvent.Users = new List<User>();

                this._currentEvent.UserEvents = this._currentEvent.UserEvents ?? new List<UserEvent>();
                this._currentEvent.UserEvents.Add(userEvent);

                return this._currentEvent;
            }
            set
            {
                this._currentEvent = value;

                UserEvent userEvent = new UserEvent();
                userEvent.EventId = this._currentEvent.Id;
                userEvent.EventType = this._currentEvent.EventType;
                userEvent.Events = new List<Event>();
                userEvent.Events.Add(this._currentEvent);

                //Todo global constant
                userEvent.Status = new UserEventStatus
                {
                    Id = 0,
                    Status = "unknown"
                };
                userEvent.Users = new List<User>();

                this._currentEvent.UserEvents = this._currentEvent.UserEvents?? new List<UserEvent>();
                this._currentEvent.UserEvents.Add(userEvent);


                CurrentEventVm = new EventViewModel()
                {
                    Title = this.CurrentEvent.Name,
                    Event = this.CurrentEvent,
                    Cover = null
                };
                _currentEventViewModel.IsBusy = true;
                Task.Factory.StartNew(() =>
                {
                    if(!String.IsNullOrEmpty(this.CurrentEvent.Cover.Url))
                        _currentEventViewModel.Cover =
                            this._service.GetCover(this.CurrentEvent.Cover.Url, ThumbnailSize.Medium).Result;
                    _currentEventViewModel.IsBusy = false;
                });

                this.OnPropertyChanged();
            }
        }

        public ICommand YesCommand
        {
            get
            {
                return this._yesCommand ??
                       (this._yesCommand =
                           new Command(
                               () => 
                               {
                                   //Todo global constant
                                   UserEventStatus acceptedUserStatus = new UserEventStatus
                                   {
                                       Id = 1,
                                       Status = "accepted"
                                   };

                                   //ToDo currently we are not getting UserEvent from the server, so first also will contain 
                                   //UserEvent that is created on mobile device for current user
                                   this._currentEvent.UserEvents.FirstOrDefault().Status = acceptedUserStatus;
                                   this.IncrementCurrentEvent();
                               }));
            }
        }

        public ICommand NoCommand => this._noCommand ??
                                     (this._noCommand =
                                         new Command(
                                             () =>
                                             {
                                                 //Todo global constant
                                                 UserEventStatus declinedUserStatus = new UserEventStatus
                                                 {
                                                     Id = 2,
                                                     Status = "declined"
                                                 };

                                                 //ToDo currently we are not getting UserEvent from the server, so first also will contain 
                                                 //UserEvent that is created on mobile device for current user
                                                 this._currentEvent.UserEvents.FirstOrDefault().Status = declinedUserStatus;
                                                 this.IncrementCurrentEvent();
                                             }));

        private void IncrementCurrentEvent()
        {
            this._localDataService.AddItem(this.CurrentEvent);
            var newIndex = this.Events.IndexOf(this.CurrentEvent) + 1;
            if (newIndex < this.Events.Count)
            {
                this.CurrentEvent = this.Events.ElementAtOrDefault(newIndex);
                return;
            }

            this.HasEvents = false;
        }

        public bool HasEvents
        {
            get { return this._hasEvents; }
            set
            {
                this._hasEvents = value;

                InfoVisible = !value;
                InfoString = value ? "" : "No Events Available";

                this.OnPropertyChanged();
            }
        }

        public bool InfoVisible
        {
            get { return this._infoVisible; }
            private set
            {
                _infoVisible = value;
                OnPropertyChanged(propertyName: nameof(InfoVisible));
            }
        }

        public string InfoString
        {
            get { return this._infoString; }
            set
            {
                this._infoString = value;
                OnPropertyChanged(propertyName: nameof(InfoString));
            }
        }

        public ICommand SelectCommand
        {
            get
            {
                return this._selectCommand ??
                       (this._selectCommand = new Command(() => this._navigator.PushAsync<EventViewModel>(CurrentEventVm)));
            }
        }
    }
}
