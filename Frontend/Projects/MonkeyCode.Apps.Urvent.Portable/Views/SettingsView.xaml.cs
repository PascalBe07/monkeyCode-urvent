using MonkeyCode.Apps.Urvent.Portable.ViewModels;
using Xamarin.Forms;

namespace MonkeyCode.Apps.Urvent.Portable.Views
{
    public partial class SettingsView : ContentPage
    {
        public SettingsView(SettingsViewModel viewModel)
        {
            InitializeComponent();
            this.BindingContext = viewModel;
        }
    }
}
