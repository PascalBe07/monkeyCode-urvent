using Autofac;
using MonkeyCode.Framework.Portable.XamarinForms.DependencyInjection.Core;
using Xamarin.Forms;

namespace MonkeyCode.Framework.Portable.XamarinForms.DependencyInjection
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // service registration
            builder.RegisterType<ViewFactory>()
                .As<IViewFactory>()
                .SingleInstance();

            builder.RegisterType<Navigator>()
                .As<INavigator>()
                .SingleInstance();

            // navigation registration
            builder.Register(context =>
            {
                var mainPage = Application.Current.MainPage;
                var masterDetailPage = mainPage as MasterDetailPage;
                if (masterDetailPage != null)
                {
                    mainPage = masterDetailPage.Detail;
                    //masterDetailPage.IsPresented = false;
                }

                return mainPage.Navigation;
            }).SingleInstance();

            builder.Register<IMasterNavigation>(context => new MasterNavigation(p =>
            {
                var masterDetail = Application.Current.MainPage as MasterDetailPage;
                if (masterDetail != null)
                {
                    masterDetail.Detail = p;
                    masterDetail.IsPresented = false;
                }
            })).SingleInstance();
        }
    }
}