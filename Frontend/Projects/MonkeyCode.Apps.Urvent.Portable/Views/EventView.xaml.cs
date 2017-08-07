using MonkeyCode.Apps.Urvent.Portable.ViewModels;
using Xamarin.Forms;

namespace MonkeyCode.Apps.Urvent.Portable.Views
{
    public partial class EventView : ContentPage
    {
        public EventView(EventViewModel viewModel)
        {
            InitializeComponent();
            this.BindingContext = viewModel;
        }
    }
}
