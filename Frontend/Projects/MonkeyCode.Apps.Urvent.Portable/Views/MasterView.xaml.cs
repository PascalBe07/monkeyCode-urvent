using MonkeyCode.Apps.Urvent.Portable.ViewModels;
using Xamarin.Forms;

namespace MonkeyCode.Apps.Urvent.Portable.Views
{
    public partial class MasterView : ContentPage
    {
        public MasterView(MasterViewModel viewModel)
        {
            InitializeComponent();
            this.BindingContext = viewModel;
        }
    }
}
