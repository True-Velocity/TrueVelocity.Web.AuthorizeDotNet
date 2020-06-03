using System;
using System.Diagnostics;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Xml.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bet.Extensions.AuthorizeNet
{
    public class CustomContractResolver : DefaultContractResolver
    {
        private readonly JsonMediaTypeFormatter _formatter;

        public CustomContractResolver(JsonMediaTypeFormatter formatter)
        {
            _formatter = formatter;
        }

        public JsonMediaTypeFormatter Formatter
        {
            [DebuggerStepThrough]
            get => _formatter;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            ConfigureProperty(member, property);
            return property;
        }

        private void ConfigureProperty(MemberInfo member, JsonProperty property)
        {
            if (Attribute.IsDefined(member, typeof(XmlIgnoreAttribute), true))
            {
                property.Ignored = true;
            }
        }
    }
}
