using MonkeyCode.Apps.Urvent.Portable.ViewModels;
using Xamarin.Forms;

namespace MonkeyCode.Apps.Urvent.Portable.Views
{
    public partial class EventsView : ContentPage
    {
        public EventsView(EventsViewModel viewModel)
        {
            InitializeComponent();
            this.BindingContext = viewModel;
        }
    }
}
