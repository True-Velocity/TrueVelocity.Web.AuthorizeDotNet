using System.Text;
using System.Xml;
using System.Xml.Serialization;

using AuthorizeNet.Api.V1.Contracts;

using Bet.Extensions.AuthorizeNet.Logging;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace Bet.Extensions.AuthorizeNet.Api.V1.Clients;

public class AuthorizeNetClient<TRequest, TResponse>
    : IAuthorizeNetClient<TRequest, TResponse> where TRequest : ANetApiRequest where TResponse : ANetApiResponse
{
    private readonly MerchantAuthenticationType _merchantAuthenticationType;
    private readonly ILogger<AuthorizeNetClient<TRequest, TResponse>> _logger;
    private readonly HttpClient _httpClient;

    public AuthorizeNetClient(
        MerchantAuthenticationType merchantAuthenticationType,
        IHttpClientFactory httpClientFactory,
        ILogger<AuthorizeNetClient<TRequest, TResponse>> logger)
    {
        _merchantAuthenticationType = merchantAuthenticationType ?? throw new ArgumentNullException(nameof(merchantAuthenticationType));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<TResponse> PostAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        request.MerchantAuthentication = _merchantAuthenticationType;

        var serializer = new XmlSerializer(request.GetType());
        using var streamXml = new MemoryStream();
        serializer.Serialize(streamXml, request, new XmlSerializerNamespaces());
        streamXml.Position = 0;

        var doc = new XmlDocument();
        doc.Load(streamXml);

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            var xmlString = request.GetXml();
            _logger.LogDebug(xmlString.MaskSensitiveXmlString());
        }

        var json = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented, false);

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug(json.MaskSensitiveString());
        }

        using var requestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        using var response = await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug(responseString);
        }

        return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync()) !;
    }
}
