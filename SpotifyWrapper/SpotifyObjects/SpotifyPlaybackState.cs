using System.Text.Json.Nodes;

namespace SpotifyWrapper.SpotifyObjects;

public class SpotifyPlaybackState
{
    private readonly JsonNode _jsonNode;

    public string? CurrentlyPlayingType => _jsonNode["currently_playing_type"]?.ToString();
    public string? RepeatState => _jsonNode["repeat_state"]?.ToString();

    public bool? ShuffleState => bool.Parse(_jsonNode["shuffle_state"]?.ToString()!);
    public bool? IsPlaying => bool.Parse(_jsonNode["is_playing"]?.ToString()!);
    public int ProgressMs => int.Parse(_jsonNode["progress_ms"]?.ToString()!);

    public SpotifyContext? Context => new SpotifyContext(_jsonNode["context"]!);

    public SpotifyItem? Item
    {
        get
        {
            return _jsonNode["currently_playing_type"]?.ToString() switch
            {
                "track" => new SpotifyTrack(_jsonNode["item"]!),
                "episode" => new SpotifyEpisode(_jsonNode["episode"]!),
                _ => null
            };
        }
    }
    public SpotifyDevice? Device => new SpotifyDevice(_jsonNode["device"]!);

    public SpotifyPlaybackState(JsonNode jsonNode)
    {
        _jsonNode = jsonNode;
    }
}