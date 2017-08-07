using Foundation;
using MonkeyCode.Apps.Urvent.Portable.Application;
using UIKit;
using Xamarin.Forms;
using HockeyApp.iOS;

namespace MonkeyCode.Apps.Urvent.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            var manager = BITHockeyManager.SharedHockeyManager;
            manager.Configure("c3bee0c1227d4b0698ca78a1e29eff9a");
            manager.StartManager();
            //manager.Authenticator.AuthenticateInstallation(); // This line is obsolete in crash only builds

            Forms.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(application, launchOptions);
        }
    }
}


