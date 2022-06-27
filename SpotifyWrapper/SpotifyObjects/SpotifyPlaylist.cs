using System.Text.Json.Nodes;

namespace SpotifyWrapper.SpotifyObjects;

public class SpotifyPlaylist
{
    private readonly JsonNode _jsonNode;

    public string? Id => _jsonNode["id"]?.ToString();
    public string? Name => _jsonNode["name"]?.ToString();
    public string? Href => _jsonNode["href"]?.ToString();
    public string? Description => _jsonNode["description"]?.ToString();
    public string? Type => _jsonNode["type"]?.ToString();
    public string? Uri => _jsonNode["uri"]?.ToString();

    public bool? Collaborative => bool.Parse(_jsonNode["collaborative"]?.ToString()!);
    public SpotifyFollowers Followers => new SpotifyFollowers(_jsonNode["followers"]!);
    public SpotifyUser? Owner => new SpotifyUser(_jsonNode["owner"]!);
    public List<SpotifyImage>? Images =>
        _jsonNode["images"]?.AsArray().Select(image => new SpotifyImage(image!)).ToList();

    public List<SpotifyPlaylistItem>? Items =>
        _jsonNode["tracks"]?["items"]?.AsArray().Select(track => new SpotifyPlaylistItem(track!)).ToList();

    public SpotifyPlaylist(JsonNode jsonNode)
    {
        _jsonNode = jsonNode;
    }
}