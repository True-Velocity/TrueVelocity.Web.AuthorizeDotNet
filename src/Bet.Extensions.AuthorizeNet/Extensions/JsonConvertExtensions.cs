using Newtonsoft.Json;

namespace Bet.Extensions.AuthorizeNet;

public static class JsonConvertExtensions
{
    public static T FromJson<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json, JsonConvertSettings.Settings) !;
    }

    public static string ToJson<T>(T instance)
    {
        return JsonConvert.SerializeObject(instance, JsonConvertSettings.Settings);
    }
}
