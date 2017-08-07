using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonkeyCode.Framework.Portable.Urvent.Models;
using MonkeyCode.Framework.Portable.Urvent.Services;

namespace MonkeyCode.Apps.Urvent.Portable.ViewModels
{
    public class MyEventsViewModel : Framework.Portable.ViewModel.ViewModel
    {
        //private readonly INavigator _navigator;
        private readonly IDataService _localDataService;
        private readonly IEventService _eventService;
        private EventViewModel[] _events;
        //private ICommand _openEvent;

        public MyEventsViewModel(IDataService localDataService, IEventService eventService) //, INavigator navigator)
        {
            //_navigator = navigator;
            _localDataService = localDataService;
            _eventService = eventService;
            this.Title = "My events";
        }

        public EventViewModel[] Events
        {
            get
            {
                List<Event> acceptedEvents = new List<Event>();

                
                // ReSharper disable once LoopCanBeConvertedToQuery (nearly unreadable as LINQ)
                foreach (var savedEvent in _localDataService.GetItems<Event>())
                {
                    if (savedEvent.UserEvents.Any())
                    {
                        //ToDo use global accepted status
                        //ToDo currently we are not getting UserEvent from the server, so first also will contain 
                        //UserEvent that is created on mobile device for current user
                        if (savedEvent.UserEvents.FirstOrDefault().Status.Id.Equals(1))
                        {
                            acceptedEvents.Add(savedEvent);
                        }
                        
                    }
                }

               
                this._events = acceptedEvents.Select(ev =>
                {
                    var evvm = new EventViewModel
                    {
                        Event = ev,
                        Title = ev.Name,
                        Cover = null,
                        IsBusy = true
                    };
                    Task.Factory.StartNew(() =>
                    {
                        if (!String.IsNullOrEmpty(ev.Cover.Url))
                            evvm.Cover =
                                this._eventService.GetCover(ev.Cover.Url, ThumbnailSize.Small).Result;
                        evvm.IsBusy = false;
                    });
                    return evvm;
                }).ToArray();

                return this._events;
            }
        }

        //public ICommand OpenEvent
        //{
        //    get
        //    {
        //        return this._openEvent ?? (this._openEvent = new Command(o => _navigator.PushAsync<EventViewModel>(
        //            vm =>
        //            {
        //                var selectedEvent = (Event) o;
        //                vm.Title = selectedEvent.Name.Text;
        //                vm.Event = selectedEvent;
        //            })));
        //    }
        //}
    }
}