using System.Text;
using System.Text.Json.Nodes;
using System.Web;
using System.Xml.Serialization;

namespace SpotifyWrapper;

public class Authenticator
{   
    // Tokens
    private string? _accessToken;
    private string? _refreshToken;
    private DateTime? _expirationDateTime;
    
    // Credentials
    private readonly string _clientId;
    private readonly string _clientSecret;
    
    // Http Client
    private readonly HttpClient _httpClient = new();
    
    // Xml serializer
    private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(AuthenticatorCache));
    
    // Redirect URI
    private readonly string _redirectUri;
    
    // Scope
    private readonly string _scope;
    
    // Cache file
    private readonly string _cacheFile;
    
    
    // Token property
    public string? AccessToken
    {
        get
        {
            if (_expirationDateTime < DateTime.Now) RefreshAccessToken();
            return _accessToken;
        }
    }

    
    // Constructor
    public Authenticator(string clientId, string clientSecret, string redirectUri, string scope, string cacheFile)
    {
        _clientId = clientId;
        _clientSecret = clientSecret;
        _redirectUri = redirectUri;
        _scope = scope;
        _cacheFile = cacheFile;

        if (!File.Exists(cacheFile)) RequestAccessToken(RequestUserAuth());
        else
        {
            var streamReader = new StreamReader(cacheFile);
            try
            {
                if (_xmlSerializer.Deserialize(streamReader) is not AuthenticatorCache cache)
                    throw new NullReferenceException();
                streamReader.Close();
                if (string.IsNullOrEmpty(cache.AccessToken)) throw new NullReferenceException();
                _accessToken = cache.AccessToken;
                if (string.IsNullOrEmpty(cache.RefreshToken)) throw new NullReferenceException();
                _refreshToken = cache.RefreshToken;
                if (!scope.Equals(cache.Scope)) throw new Exception("Scope changed");
                _scope = cache.Scope;
                _expirationDateTime = cache.ExpirationDateTime;
            }
            catch (Exception e)
            {
                streamReader.Close();
                RequestAccessToken(RequestUserAuth());
            }
        }
        

        Console.WriteLine("Authenticator initialized!");
    }
    
    
    // Request user authorization
    private string RequestUserAuth()
    {
        // Build request query string
        var requestQuery = HttpUtility.ParseQueryString(string.Empty);
        requestQuery.Add("client_id", _clientId);
        requestQuery.Add("redirect_uri", _redirectUri);
        requestQuery.Add("response_type", "code");
        requestQuery.Add("scope", _scope);
        
        // Build and print request uri
        var requestUri = SpotifyWrapper.AuthorizeEndpoint + "?" + requestQuery;
        Console.WriteLine(requestUri);
        
        // Extract code from callback uri
        var callbackUri = new Uri(Console.ReadLine() ?? string.Empty);
        return HttpUtility.ParseQueryString(callbackUri.Query).Get("code") ?? throw new Exception(
            "Could not extract code from redirect uri");
    }
    
    // Request access token
    private void RequestAccessToken(string code)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, SpotifyWrapper.TokenEndpoint);
        requestMessage.Content = new FormUrlEncodedContent(new Dictionary<string, string>()
        {
            { "grant_type", "authorization_code" },
            { "code", code },
            { "redirect_uri", _redirectUri }
        });
        requestMessage.Headers.Add(
            "Authorization",
            "Basic " + Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(
                _clientId + ":" + _clientSecret)));

        var responseMessage = _httpClient.Send(requestMessage);
        var jsonString = responseMessage.Content.ReadAsByteArrayAsync();
        var json = JsonNode.Parse(jsonString.Result) ?? throw new InvalidOperationException("No json object provided");

        _accessToken = (string) json["access_token"]! ?? throw new InvalidOperationException("No access token provided");
        _refreshToken = (string) json["refresh_token"]! ??
                        throw new InvalidOperationException("No refresh token provided");
        _expirationDateTime = DateTime.Now + TimeSpan.FromSeconds((int)(json["expires_in"] ??
                                                                        throw new InvalidOperationException(
                                                                            "No expiration time provided")));
        SaveAuthenticator();
    }
    
    // Refresh access token
    private void RefreshAccessToken()
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, SpotifyWrapper.TokenEndpoint);
        requestMessage.Content = new FormUrlEncodedContent(new Dictionary<string, string>()
        {
            {"grant_type", "refresh_token"},
            {"refresh_token", _refreshToken ?? throw new InvalidOperationException("No refresh token provided")}
        });
        requestMessage.Headers.Add(
            "Authorization",
            "Basic " + Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(
                _clientId + ":" + _clientSecret)));
    
        var responseMessage = _httpClient.Send(requestMessage);
        var json = JsonNode.Parse(responseMessage.Content.ReadAsByteArrayAsync().Result);
    
        _accessToken = (string) json["access_token"] ?? throw new InvalidOperationException(
            "No access token provided");
        _expirationDateTime = DateTime.Now + TimeSpan.FromSeconds((int)(json["expires_in"] ?? throw new InvalidOperationException(
            "No expiration time provided")));
        
        SaveAuthenticator();
    }
    
    // Save authenticator
    private void SaveAuthenticator()
    {
        var xmlSerializer = new XmlSerializer(typeof(AuthenticatorCache));
        var writer = new StreamWriter(_cacheFile);
        xmlSerializer.Serialize(writer, new AuthenticatorCache()
        {
            AccessToken = _accessToken!,
            RefreshToken = _refreshToken!,
            ExpirationDateTime = _expirationDateTime!.Value,
            Scope = _scope
        });
        writer.Close();
    }
}