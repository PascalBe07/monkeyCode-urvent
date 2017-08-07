using System;
using System.Linq;
using System.Reflection;
using SQLiteNetExtensions.Extensions;

namespace MonkeyCode.Framework.Tests.Urvent.Core
{
    internal enum Cities
    {
        [City(Latitude = 49.45, Longitude = 11.08)]
        Nürnberg,
        
        [City(Latitude = 49.441, Longitude = 11.862)]
        Amberg,

        [City(Latitude = 49.02, Longitude = 12.08)]
        Regensburg,

        [City(Latitude = 48.14, Longitude = 11.58)]
        München,

        [City(Latitude = 0, Longitude = 0)]
        None,
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    internal sealed class CityAttribute : Attribute
    {
        public double Latitude;
        public double Longitude;
    }

    internal static class CitiesExtensions
    {

        internal static Tuple<double, double> GetLocation(this Cities city)
        {
            var attribute = typeof(Cities).GetMember(city.ToString()).Single().GetCustomAttribute<CityAttribute>();
            return new Tuple<double, double>(attribute.Latitude, attribute.Longitude);
        }
    }
}
