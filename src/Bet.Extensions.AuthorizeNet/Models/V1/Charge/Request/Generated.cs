﻿// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using Bet.Extensions.AuthorizeNet.Models.V1.Charge.Request;
//
//    var chargeTransactionRequest = ChargeTransactionRequest.FromJson(jsonString);

namespace Bet.Extensions.AuthorizeNet.Models.V1.Charge.Request
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ChargeTransactionRequest
    {
        [JsonProperty("createTransactionRequest")]
        public CreateTransactionRequest CreateTransactionRequest { get; set; }
    }

    public partial class CreateTransactionRequest
    {
        [JsonProperty("merchantAuthentication")]
        public MerchantAuthentication MerchantAuthentication { get; set; }

        [JsonProperty("refId")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public long RefId { get; set; }

        [JsonProperty("transactionRequest")]
        public TransactionRequest TransactionRequest { get; set; }
    }

    public partial class MerchantAuthentication
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("transactionKey")]
        public string TransactionKey { get; set; }
    }

    public partial class TransactionRequest
    {
        [JsonProperty("transactionType")]
        public string TransactionType { get; set; } = "authCaptureTransaction";

        [JsonProperty("amount")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public long Amount { get; set; }

        [JsonProperty("payment")]
        public Payment Payment { get; set; }

        [JsonProperty("lineItems")]
        public LineItems LineItems { get; set; }

        [JsonProperty("tax")]
        public Duty Tax { get; set; }

        [JsonProperty("duty")]
        public Duty Duty { get; set; }

        [JsonProperty("shipping")]
        public Duty Shipping { get; set; }

        [JsonProperty("poNumber")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public long PoNumber { get; set; }

        [JsonProperty("customer")]
        public Customer Customer { get; set; }

        [JsonProperty("billTo")]
        public To BillTo { get; set; }

        [JsonProperty("shipTo")]
        public To ShipTo { get; set; }

        [JsonProperty("customerIP")]
        public string CustomerIp { get; set; }

        [JsonProperty("transactionSettings")]
        public TransactionSettings TransactionSettings { get; set; }

        [JsonProperty("userFields")]
        public UserFields UserFields { get; set; }

        [JsonProperty("processingOptions")]
        public ProcessingOptions ProcessingOptions { get; set; }

        [JsonProperty("subsequentAuthInformation")]
        public SubsequentAuthInformation SubsequentAuthInformation { get; set; }

        [JsonProperty("authorizationIndicatorType")]
        public AuthorizationIndicatorType AuthorizationIndicatorType { get; set; }
    }

    public partial class AuthorizationIndicatorType
    {
        [JsonProperty("authorizationIndicator")]
        public string AuthorizationIndicator { get; set; }
    }

    public partial class To
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("zip")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public long Zip { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }

    public partial class Customer
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class Duty
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public partial class LineItems
    {
        [JsonProperty("lineItem")]
        public LineItem LineItem { get; set; }
    }

    public partial class LineItem
    {
        [JsonProperty("itemId")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public long ItemId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("quantity")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public long Quantity { get; set; }

        [JsonProperty("unitPrice")]
        public string UnitPrice { get; set; }
    }

    public partial class Payment
    {
        [JsonProperty("creditCard")]
        public CreditCard CreditCard { get; set; }
    }

    public partial class CreditCard
    {
        [JsonProperty("cardNumber")]
        public string CardNumber { get; set; }

        [JsonProperty("expirationDate")]
        public string ExpirationDate { get; set; }

        [JsonProperty("cardCode")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public long CardCode { get; set; }
    }

    public partial class ProcessingOptions
    {
        [JsonProperty("isSubsequentAuth")]
        [JsonConverter(typeof(FluffyParseStringConverter))]
        public bool IsSubsequentAuth { get; set; }
    }

    public partial class SubsequentAuthInformation
    {
        [JsonProperty("originalNetworkTransId")]
        public string OriginalNetworkTransId { get; set; }

        [JsonProperty("originalAuthAmount")]
        public string OriginalAuthAmount { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }

    public partial class TransactionSettings
    {
        [JsonProperty("setting")]
        public Setting Setting { get; set; }
    }

    public partial class Setting
    {
        [JsonProperty("settingName")]
        public string SettingName { get; set; }

        [JsonProperty("settingValue")]
        [JsonConverter(typeof(FluffyParseStringConverter))]
        public bool SettingValue { get; set; }
    }

    public partial class UserFields
    {
        [JsonProperty("userField")]
        public UserField[] UserField { get; set; }
    }

    public partial class UserField
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class ChargeTransactionRequest
    {
        public static ChargeTransactionRequest FromJson(string json) => JsonConvert.DeserializeObject<ChargeTransactionRequest>(json, Bet.Extensions.AuthorizeNet.Models.V1.Charge.Request.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this ChargeTransactionRequest self) => JsonConvert.SerializeObject(self, Bet.Extensions.AuthorizeNet.Models.V1.Charge.Request.Converter.Settings);
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

    internal class PurpleParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly PurpleParseStringConverter Singleton = new PurpleParseStringConverter();
    }

    internal class FluffyParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(bool) || t == typeof(bool?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            bool b;
            if (Boolean.TryParse(value, out b))
            {
                return b;
            }
            throw new Exception("Cannot unmarshal type bool");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (bool)untypedValue;
            var boolString = value ? "true" : "false";
            serializer.Serialize(writer, boolString);
            return;
        }

        public static readonly FluffyParseStringConverter Singleton = new FluffyParseStringConverter();
    }
}