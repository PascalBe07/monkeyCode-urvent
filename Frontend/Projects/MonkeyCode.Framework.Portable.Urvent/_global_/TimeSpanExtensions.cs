using System;


// ReSharper disable once CheckNamespace
public static class TimeSpanExtensions
{
    public static TimeSpan Hours(this int source)
    {
        var result = new TimeSpan(source,0,0);
        return result;
    }
}