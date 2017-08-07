using System.Globalization;

// ReSharper disable once CheckNamespace
public static class DoubleExtensions
{
    public static string InvariantString(this double source)
    {
        return source.ToString(CultureInfo.InvariantCulture);
    }
}