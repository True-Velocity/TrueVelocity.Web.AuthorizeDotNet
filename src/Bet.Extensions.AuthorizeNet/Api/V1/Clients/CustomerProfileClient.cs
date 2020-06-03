using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AuthorizeNet.Api.V1.Contracts;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace Bet.Extensions.AuthorizeNet.Api.V1.Clients
{
    public class CustomerProfileClient
    {
        private readonly MerchantAuthenticationType _merchantAuthenticationType;
        private readonly ILogger<CustomerProfileClient> _logger;
        private readonly HttpClient _httpClient;

        public CustomerProfileClient(
            MerchantAuthenticationType merchantAuthenticationType,
            IHttpClientFactory httpClientFactory,
            ILogger<CustomerProfileClient> logger)
        {
            _merchantAuthenticationType = merchantAuthenticationType ?? throw new ArgumentNullException(nameof(merchantAuthenticationType));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<CreateCustomerProfileResponse> CreateCustomerAsync(CancellationToken cancellationToken = default)
        {
            var profiles = new Collection<CustomerPaymentProfileType>
            {
                new CustomerPaymentProfileType
                {
                    CustomerType = CustomerTypeEnum.Business
                }
            };

            var request = new CreateCustomerProfileRequest
            {
                ClientId = "12",
                Profile = new CustomerProfileType
                {
                    Description = "Test Customer Account",
                    Email = "email1@email.com",
                    MerchantCustomerId = "CustomerId-1",
                    ProfileType = CustomerProfileTypeEnum.Regular,
                    PaymentProfiles = profiles,
                },

                ValidationMode = ValidationModeEnum.TestMode
            };

            request.MerchantAuthentication = _merchantAuthenticationType;

            using var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
            };

            var json = JsonConvert.SerializeObject(
                request,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new CustomContractResolver(new System.Net.Http.Formatting.JsonMediaTypeFormatter())
                });

            _logger.LogDebug(json);

            requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

            // using var httpContent = CreateHttpContent(request);
            // requestMessage.Content = httpContent;
            using var response = await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<CreateCustomerProfileResponse>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// https://johnthiriet.com/efficient-post-calls/.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="stream"></param>
        private static void SerializeJsonIntoStream(object value, Stream stream)
        {
            using var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true);
            using var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None };
            var js = new JsonSerializer();
            js.Serialize(jtw, value);
            jtw.Flush();
        }

        private static HttpContent? CreateHttpContent(object content)
        {
            HttpContent? httpContent = null;

            if (content != null)
            {
                var ms = new MemoryStream();
                SerializeJsonIntoStream(content, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return httpContent;
        }
    }
}
