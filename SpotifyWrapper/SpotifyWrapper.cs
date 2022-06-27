using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;
using SpotifyWrapper.SpotifyObjects;

namespace SpotifyWrapper;

public class SpotifyWrapper
{
    // API Endpoints
    public const string AuthorizeEndpoint = "https://accounts.spotify.com/authorize/";
    public const string TokenEndpoint = "https://accounts.spotify.com/api/token/";
    private const string PlayerEndpoint = "https://api.spotify.com/v1/me/player/";
    private const string PlaylistUserEndpoint = "https://api.spotify.com/v1/me/playlists/";
    private const string PlaylistEndpoint = "https://api.spotify.com/v1/playlists/";
    private const string TrackEndpoint = "https://api.spotify.com/v1/tracks/";
    private const string AlbumEndpoint = "https://api.spotify.com/v1/albums/";
    
    // Authenticator
    private readonly Authenticator _authenticator;
    
    // Http Client
    private readonly HttpClient _httpClient = new();
    
    
    
    // Initialize Spotify Wrapper
    public SpotifyWrapper(string clientId, string clientSecret, string redirectUri, List<string> scopeList, string cacheFile)
    {
        var scope = "";
        foreach (var s in scopeList)
        {
            if ("".Equals(scope)) scope = s;
            else scope = scope + " " + s;
        }
        
        // Initialize authenticator
        _authenticator = new Authenticator(
            clientId,
            clientSecret,
            redirectUri,
            scope,
            cacheFile);

        Console.WriteLine("Spotify wrapper initialized!");
    }

    private HttpResponseMessage SendApiRequest(HttpRequestMessage requestMessage)
    {
        requestMessage.Headers.Add(
            "Authorization",
            "Bearer " + _authenticator.AccessToken);
        return _httpClient.Send(requestMessage);
    }

    // Player endpoint
    public SpotifyCurrentlyPlaying GetCurrentlyPlaying()
    {
        return new SpotifyCurrentlyPlaying(JsonNode.Parse(SendApiRequest(new HttpRequestMessage(HttpMethod.Get, PlayerEndpoint + "currently-playing"))
            .Content.ReadAsByteArrayAsync().Result)!);
    }

    public HttpResponseMessage GetUserPlaylists()
    {
        return SendApiRequest(new HttpRequestMessage(HttpMethod.Get, PlaylistUserEndpoint));
    }

    public SpotifyTrack GetTrack(string id)
    {
        return new SpotifyTrack(JsonNode.Parse(SendApiRequest(new HttpRequestMessage(HttpMethod.Get, TrackEndpoint + id))
            .Content.ReadAsByteArrayAsync().Result)!);
    }

    public SpotifyAlbum GetAlbum(string id)
    {
        return new SpotifyAlbum(JsonNode.Parse(SendApiRequest(new HttpRequestMessage(HttpMethod.Get, AlbumEndpoint + id))
            .Content.ReadAsByteArrayAsync().Result)!);
    }

    public SpotifyPlaylist GetPlaylist(string id)
    {
        var playlistJson = JsonNode.Parse(SendApiRequest(new HttpRequestMessage(HttpMethod.Get, PlaylistEndpoint + id))
            .Content.ReadAsByteArrayAsync().Result);

        var nextUri = playlistJson?["tracks"]?["next"]?.ToString();

        while (nextUri != null)
        {
            var tracks = JsonNode.Parse(SendApiRequest(new HttpRequestMessage(HttpMethod.Get, nextUri)).Content
                .ReadAsByteArrayAsync().Result);

            for (var i = 0; i < tracks?["items"]?.AsArray().Count; i++)
            {
                var item = JsonNode.Parse(tracks["items"]?[i]?.ToJsonString()!);
                playlistJson?["tracks"]?["items"]?.AsArray().Add(item);
            }

            nextUri = tracks?["next"]?.ToString();
        }

        return new SpotifyPlaylist(playlistJson!);
    }

    public SpotifyPlaybackState GetPlaybackState()
    {
        return new SpotifyPlaybackState(JsonNode.Parse(SendApiRequest(new HttpRequestMessage(HttpMethod.Get, PlayerEndpoint))
                .Content.ReadAsByteArrayAsync().Result)!);
    }
}