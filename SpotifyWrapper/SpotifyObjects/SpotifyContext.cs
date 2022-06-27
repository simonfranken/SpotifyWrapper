using System.Text.Json.Nodes;

namespace SpotifyWrapper.SpotifyObjects;

public class SpotifyContext
{
    private readonly JsonNode _jsonNode;

    public string? Type => _jsonNode["type"]?.ToString();
    public string? Href => _jsonNode["href"]?.ToString();
    public string? Uri => _jsonNode["uri"]?.ToString();

    public SpotifyExternalUrls ExternalUrls => new SpotifyExternalUrls(_jsonNode["external_urls"]!);

    public SpotifyContext(JsonNode jsonNode)
    {
        _jsonNode = jsonNode;
    }
}