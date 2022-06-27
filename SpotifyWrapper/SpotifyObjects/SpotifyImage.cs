using System.Text.Json.Nodes;

namespace SpotifyWrapper.SpotifyObjects;

public class SpotifyImage
{
    private readonly JsonNode _jsonNode;

    public string? Url => _jsonNode["url"]?.ToString();

    public int Height => int.Parse(_jsonNode["height"]?.ToString()!);
    public int Width => int.Parse(_jsonNode["width"]?.ToString()!);

    public SpotifyImage(JsonNode jsonNode)
    {
        _jsonNode = jsonNode;
    }
}