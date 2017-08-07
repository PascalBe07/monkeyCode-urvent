using System;
using System.Globalization;
using Newtonsoft.Json.Linq;

// ReSharper disable once CheckNamespace
public static class JsonExtensions
{
    public static string TokenString(this JToken token, string path)
    {
        var result = (string) token.SelectToken(path);
        return result;
    }

    public static TResult TokenValue<TResult>(this JToken token, string path, TResult fallBack = default(TResult))
        where TResult : struct
    {
        var valueToken = token.SelectToken(path);
        var result = valueToken?.Value<TResult>() ?? fallBack;
        return result;
    }

    public static DateTime TokenDateTime(this JToken token, string path, string format = null)
    {
        var dateTimeString = token.TokenString(path);
        if (dateTimeString == null) return default(DateTime);
        var result = DateTime.ParseExact(dateTimeString, format ?? "O", CultureInfo.InvariantCulture);
        return result;
    }
}
