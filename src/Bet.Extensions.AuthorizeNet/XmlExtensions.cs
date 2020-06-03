using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Bet.Extensions.AuthorizeNet
{
    public static class XmlExtensions
    {
        public static string SerializeObject<T>(this T toSerialize)
        {
            if (toSerialize != null)
            {
                var xmlSerializer = new XmlSerializer(toSerialize.GetType());

                using var textWriter = new StringWriter();
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }

            return string.Empty;
        }

        public static T Deserialize<T>(this string value)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            return (T)xmlSerializer.Deserialize(new StringReader(value));
        }

        public static string Serialize<T>(this T value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var xmlSerializer = new XmlSerializer(typeof(T));

            using var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true });
            xmlSerializer.Serialize(xmlWriter, value);
            return stringWriter.ToString();
        }

        public static string GetXml<T>(this T entity)
        {
            string xmlString;

            var requestType = typeof(T);
            try
            {
                var serializer = new XmlSerializer(requestType);
                using var writer = new Utf8StringWriter();
                serializer.Serialize(writer, entity);
                xmlString = writer.ToString();
            }
            catch (Exception e)
            {
                throw;
            }

            return xmlString;
        }

        public static T Create<T>(this string xml)
        {
            var entity = default(T);

            // make sure we have not null and not-empty string to de-serialize
            if (!string.IsNullOrEmpty(xml)
                && xml.Trim().Length != 0)
            {
                var responseType = typeof(T);
                try
                {
                    object deSerializedobject;
                    var serializer = new XmlSerializer(responseType);
                    using (var reader = new StringReader(xml))
                    {
                        deSerializedobject = serializer.Deserialize(reader);
                    }

                    if (deSerializedobject is T t)
                    {
                        entity = t;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return entity;
        }
    }

    public sealed class Utf8StringWriter : StringWriter
    {
        public override System.Text.Encoding Encoding => System.Text.Encoding.UTF8;
    }
}
