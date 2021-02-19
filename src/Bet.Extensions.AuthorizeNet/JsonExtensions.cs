using System.Globalization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bet.Extensions.AuthorizeNet
{
    public static class JsonExtensions
    {
        public static T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Converter.Settings);
        }

        public static string ToJson<T>(T instance)
        {
            return JsonConvert.SerializeObject(instance, Converter.Settings);
        }
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
