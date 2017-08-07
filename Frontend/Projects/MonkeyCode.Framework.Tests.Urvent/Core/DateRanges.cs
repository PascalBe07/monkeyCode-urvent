using MonkeyCode.Framework.Portable.Urvent.Models;

namespace MonkeyCode.Framework.Tests.Urvent.Core
{
    internal enum DateRanges
    {
        Today,

        Week,

        Weekend,
    }

    internal static class DateRangesExtension
    {
        public static EventDateRange ToEventDateRange(this DateRanges dateRange)
        {
            var eventDateRange = new EventDateRange {Description = dateRange.ToString()};
            return eventDateRange;
        }
    }
}