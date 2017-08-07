using MonkeyCode.Apps.Urvent.Portable.ViewModels;
using Xamarin.Forms;

namespace MonkeyCode.Apps.Urvent.Portable.Views
{
    public partial class MyEventsView : ContentPage
    {
        public MyEventsView(MyEventsViewModel viewModel)
        {
            InitializeComponent();
            this.BindingContext = viewModel;
        }
    }
}
