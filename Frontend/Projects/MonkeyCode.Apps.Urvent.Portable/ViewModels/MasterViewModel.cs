using System.Windows.Input;
using MonkeyCode.Framework.Portable.XamarinForms.DependencyInjection;
using Xamarin.Forms;

namespace MonkeyCode.Apps.Urvent.Portable.ViewModels
{
    public class MasterViewModel : Framework.Portable.ViewModel.ViewModel
    {
        private readonly INavigator _navigator;
        private ICommand _myEventsCommand;
        private ICommand _searchCommand;
        private ICommand _settingsCommand;

        public MasterViewModel(INavigator navigator)
        {
            _navigator = navigator;
            this.Title = "Urvent";
        }

        public ICommand MyEventsCommand
        {
            get
            {
                return this._myEventsCommand ?? (this._myEventsCommand = new Command(() =>
                {
                    this._navigator.SetSync<MyEventsViewModel>(m => m.OnPropertyChanged(nameof(m.Events)));
                }));
            }
        }

        public ICommand SearchEventsCommand
        {
            get
            {
                return this._searchCommand ?? (this._searchCommand = new Command(() =>
                {
                    this._navigator.SetSync<EventsViewModel>();
                }));
            }
        }

        public ICommand SettingsCommand
        {
            get
            {
                return this._settingsCommand ?? (this._settingsCommand = new Command(() =>
                {
                    this._navigator.SetSync<SettingsViewModel>();
                }));
            }

        }
    }
}
