using System;
using MonkeyCode.Apps.Urvent.Droid.CustomRenderer;
using MonkeyCode.Apps.Urvent.Portable.Application;
using MonkeyCode.Apps.Urvent.Portable.Views;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(LoginPage), typeof(LoginPageRenderer))]

namespace MonkeyCode.Apps.Urvent.Droid.CustomRenderer
{
    public class LoginPageRenderer : PageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            //var auth = new OAuth2Authenticator(
            //    clientId: "179685449034986",
            //    scope: "",
            //    authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
            //    redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));

            //auth.Completed += Auth_Completed;

            this.Context.StartActivity(App.FacebookAuthenticator.GetUI(this.Context));
        }

        //private void Auth_Completed(object sender, AuthenticatorCompletedEventArgs e)
        //{
        //    if (e.IsAuthenticated)
        //    {
        //        App.OnSuccessfullAuthentication(e.Account);
        //    }
        //}
    }
}