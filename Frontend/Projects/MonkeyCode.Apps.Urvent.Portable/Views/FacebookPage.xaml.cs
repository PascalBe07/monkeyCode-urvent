using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyCode.Apps.Urvent.Portable.Application;
using Xamarin.Forms;

namespace MonkeyCode.Apps.Urvent.Portable.Views
{
    public partial class FacebookPage : ContentPage
    {
        public FacebookPage()
        {
            InitializeComponent();
            this.BindingContext = this;
        }

        protected override void OnAppearing()
        {
            if (!App.IsLogged)
            {
                App.NavPage.PushAsync(new LoginPage());
            }
        }

        public string MainText => App.CurrentAccount?.Username;
    }
}
