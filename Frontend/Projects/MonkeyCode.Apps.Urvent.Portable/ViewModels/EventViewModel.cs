using System;
using MonkeyCode.Framework.Portable.Urvent.Models;
using Xamarin.Forms;

namespace MonkeyCode.Apps.Urvent.Portable.ViewModels
{
    public class EventViewModel : Framework.Portable.ViewModel.ViewModel
    {
        private Event _event;
        private ImageSource _cover;
        private bool _isBusy = false;
        private bool _imageLoaded = false;

        public Event Event
        {
            get { return _event; }
            set
            {
                _event = value;
                this.OnPropertyChanged(propertyName: nameof(Event));
            }
        }

        public ImageSource Cover
        {
            get { return _cover; }
            set
            {
                _cover = value;
                this.ImageLoaded = value != null;
                this.OnPropertyChanged(propertyName:nameof(Cover));
            }
        }

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                this.OnPropertyChanged(propertyName: nameof(IsBusy));
            }
        }

        public bool ImageLoaded
        {
            get { return _imageLoaded; }
            private set
            {
                _imageLoaded = value;
                OnPropertyChanged(propertyName: nameof(ImageLoaded));
            }
        }

    }
}