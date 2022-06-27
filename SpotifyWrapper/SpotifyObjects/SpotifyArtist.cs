using System.Text.Json.Nodes;

namespace SpotifyWrapper.SpotifyObjects;

public class SpotifyArtist
{
    private readonly JsonNode _jsonNode;

    public string? Id => _jsonNode["id"]?.ToString();
    public string? Name => _jsonNode["name"]?.ToString();
    public string? Type => _jsonNode["type"]?.ToString();
    public string? Uri => _jsonNode["uri"]?.ToString();
    public string? Href => _jsonNode["href"]?.ToString();

    public SpotifyExternalUrls? ExternalUrl => new SpotifyExternalUrls(_jsonNode["external_urls"]!);

    public SpotifyArtist(JsonNode jsonNode)
    {
        _jsonNode = jsonNode;
    }
}