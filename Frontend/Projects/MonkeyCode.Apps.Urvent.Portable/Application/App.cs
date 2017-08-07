using System;
using MonkeyCode.Apps.Urvent.Portable.Views;
using Xamarin.Auth;
using Xamarin.Forms;

namespace MonkeyCode.Apps.Urvent.Portable.Application
{
    public class App : Xamarin.Forms.Application
    {
        public App()
        {
            //FacebookAuthenticator.Completed += (s, a) => OnSuccessfullAuthentication(a.Account);

            //NavPage = new NavigationPage(new FacebookPage());
            //MainPage = NavPage;

            var bootstrapper = new UrventBootstrapper(this);
            bootstrapper.Run();
        }

        public static NavigationPage NavPage;

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static OAuth2Authenticator FacebookAuthenticator = new OAuth2Authenticator(
                clientId: "179685449034986",
                scope: "",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));

        public static void OnSuccessfullAuthentication(Account account)
        {
            CurrentAccount = account;
            var accessToken = account.Properties["access_token"];
            NavPage.PopAsync();
        }

        public static Account CurrentAccount { get; set; }
        public static bool IsLogged => CurrentAccount != null;
    }
}
