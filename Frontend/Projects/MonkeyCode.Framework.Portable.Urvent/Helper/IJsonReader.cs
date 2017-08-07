namespace MonkeyCode.Framework.Portable.Urvent.Services
{
    public interface IJsonReader<out TConvert>
    {
        TConvert ReadJson(string json);
    }
}