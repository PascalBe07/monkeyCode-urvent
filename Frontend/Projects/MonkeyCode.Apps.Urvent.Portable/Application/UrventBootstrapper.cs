using Autofac;
using MonkeyCode.Apps.Urvent.Portable.ViewModels;
using MonkeyCode.Apps.Urvent.Portable.Views;
using MonkeyCode.Framework.Portable.XamarinForms;
using MonkeyCode.Framework.Portable.XamarinForms.DependencyInjection;
using Xamarin.Forms;

namespace MonkeyCode.Apps.Urvent.Portable.Application
{
    public class UrventBootstrapper : AutofacBootstrapper
    {
        private readonly App _application;

        public UrventBootstrapper(App application)
        {
            _application = application;
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);
            builder.RegisterModule<UrventModule>();
        }

        protected override void RegisterViews(IViewFactory viewFactory)
        {
            viewFactory.Register<EventsViewModel, EventsView>();
            viewFactory.Register<EventViewModel, EventView>();
            viewFactory.Register<MasterViewModel, MasterView>();
            viewFactory.Register<MyEventsViewModel, MyEventsView>();
            viewFactory.Register<SettingsViewModel, SettingsView>();
        }

        protected override void ConfigureApplication(IContainer container)
        {
            //  main page
            var viewFactory = container.Resolve<IViewFactory>();
            var detailPage = viewFactory.Resolve<EventsViewModel>();
            var navigationPage = new NavigationPage(detailPage);
            var masterPage = viewFactory.Resolve<MasterViewModel>();
            var masterDetailPage = new MasterDetailPage
            {
                Master = masterPage,
                Detail = navigationPage
            };

            _application.MainPage = masterDetailPage;
        }
    }
}