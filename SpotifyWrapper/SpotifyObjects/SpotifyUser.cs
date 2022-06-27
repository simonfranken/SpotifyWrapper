using System.Text.Json.Nodes;

namespace SpotifyWrapper.SpotifyObjects;

public class SpotifyUser
{
    private readonly JsonNode _jsonNode;

    public string? Id => _jsonNode["id"]?.ToString();
    public string? DisplayName => _jsonNode["display_name"]?.ToString();
    public string? Href => _jsonNode["href"]?.ToString();
    public string? Type => _jsonNode["type"]?.ToString();
    public string? Uri => _jsonNode["uri"]?.ToString();

    public SpotifyFollowers? Followers => new SpotifyFollowers(_jsonNode["followers"]!);
    public SpotifyExternalUrls? ExternalUrls => new SpotifyExternalUrls(_jsonNode["external_urls"]!);
    public List<SpotifyImage>? Images => _jsonNode["image"]?.AsArray().Select(image => new SpotifyImage(image!)).ToList();

    public SpotifyUser(JsonNode jsonNode)
    {
        _jsonNode = jsonNode;
    }
}