using System.Text.Json.Nodes;

namespace SpotifyWrapper.SpotifyObjects;

public class SpotifyPlaylistItem
{
    private readonly JsonNode _jsonNode;

    public string? AddedAt => _jsonNode["added_at"]?.ToString();

    public SpotifyTrack Track => new SpotifyTrack(_jsonNode["track"]!);
    public bool? IsLocal => bool.Parse(_jsonNode["is_local"]!.ToString());
    public SpotifyUser AddedBy => new SpotifyUser(_jsonNode["added_by"]!);

    public SpotifyPlaylistItem(JsonNode jsonNode)
    {
        _jsonNode = jsonNode;
    }
}