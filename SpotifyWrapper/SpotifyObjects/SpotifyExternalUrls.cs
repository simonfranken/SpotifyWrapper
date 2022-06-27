using System.Text.Json.Nodes;

namespace SpotifyWrapper.SpotifyObjects;

public class SpotifyExternalUrls
{
    private readonly JsonNode _jsonNode;

    public string? Spotify => _jsonNode["spotify"]?.ToString();

    public SpotifyExternalUrls(JsonNode jsonNode)
    {
        _jsonNode = jsonNode;
    }
}