using MonkeyCode.Apps.Urvent.iOS.CustomRenderer;
using MonkeyCode.Apps.Urvent.Portable.Application;
using MonkeyCode.Apps.Urvent.Portable.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(LoginPage), typeof(LoginPageRenderer))]

namespace MonkeyCode.Apps.Urvent.iOS.CustomRenderer
{
    public class LoginPageRenderer : PageRenderer
    {
        public override void ViewDidAppear(bool animated)
        {
            //var auth = new OAuth2Authenticator(
            //    clientId: "179685449034986",
            //    scope: "",
            //    authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
            //    redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));

            //auth.Completed += Auth_Completed;

            this.PresentViewController(App.FacebookAuthenticator.GetUI(), true, null);
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