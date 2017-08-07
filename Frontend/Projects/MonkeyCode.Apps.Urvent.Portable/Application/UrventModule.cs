using Autofac;
using MonkeyCode.Apps.Urvent.Portable.Models;
using MonkeyCode.Apps.Urvent.Portable.ViewModels;
using MonkeyCode.Apps.Urvent.Portable.Views;
using MonkeyCode.Framework.Portable.Urvent.Services;
using MonkeyCode.Framework.Portable.Urvent.Services.Core;

namespace MonkeyCode.Apps.Urvent.Portable.Application
{
    public class UrventModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //  service registration
            builder.RegisterType<MockGeoLocationService>().As<IGeolocationService>().SingleInstance();
            builder.RegisterType<ModelConverterService>().As<IModelConverterService>().SingleInstance();
            builder.RegisterType<SQliteService>().As<IDataService>().SingleInstance();
            builder.RegisterType<SettingsContainer>().As<ISettingsContainer>().SingleInstance();
            builder.RegisterType<UrventService>().As<IEventService>().SingleInstance();

            //	view model registration
            builder.RegisterType<EventsViewModel>().SingleInstance();
            builder.RegisterType<EventViewModel>().SingleInstance();
            builder.RegisterType<MasterViewModel>().SingleInstance();
            builder.RegisterType<MyEventsViewModel>().SingleInstance();
            builder.RegisterType<SettingsViewModel>().SingleInstance();

            //	view registration
            builder.RegisterType<EventsView>().SingleInstance();
            builder.RegisterType<EventView>().SingleInstance();
            builder.RegisterType<MasterView>().SingleInstance();
            builder.RegisterType<MyEventsView>().SingleInstance();
            builder.RegisterType<SettingsView>().SingleInstance();

        }
    }
}