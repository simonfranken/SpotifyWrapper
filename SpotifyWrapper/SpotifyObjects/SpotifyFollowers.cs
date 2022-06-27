using System.Text.Json.Nodes;

namespace SpotifyWrapper.SpotifyObjects;

public class SpotifyFollowers
{
    private readonly JsonNode _jsonNode;

    public string? Href => _jsonNode["href"]?.ToString();
    public int Total => int.Parse(_jsonNode["total"]?.ToString()!);

    public SpotifyFollowers(JsonNode jsonNode)
    {
        _jsonNode = jsonNode;
    }
}