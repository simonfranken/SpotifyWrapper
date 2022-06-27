using System.Text.Json.Nodes;

namespace SpotifyWrapper.SpotifyObjects;

public class SpotifyCurrentlyPlaying
{
    private readonly JsonNode _jsonNode;

    public string? CurrentlyPlayingType => _jsonNode["currently_playing_type"]?.ToString();
    public bool? IsPlaying => bool.Parse(_jsonNode["is_playing"]?.ToString()!);

    public SpotifyItem? Item
    {
        get
        {
            return _jsonNode["item"]!["type"]!.ToString() switch
            {
                "track" => new SpotifyTrack(_jsonNode["item"]!),
                "episode" => new SpotifyEpisode(_jsonNode["item"]!),
                _ => null
            };
        }
    }
    
    public SpotifyCurrentlyPlaying(JsonNode jsonNode)
    {
        _jsonNode = jsonNode;
    }
}