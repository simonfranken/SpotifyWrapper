using System.Text.Json.Nodes;

namespace SpotifyWrapper.SpotifyObjects;

public class SpotifyDevice
{
    private readonly JsonNode _jsonNode;

    public string? Id => _jsonNode["id"]?.ToString();
    public string? Name => _jsonNode["name"]?.ToString();
    public string? Type => _jsonNode["type"]?.ToString();

    public bool? IsActive => bool.Parse(_jsonNode["is_active"]?.ToString()!);
    public bool? IsPrivateSession => bool.Parse(_jsonNode["is_private_session"]?.ToString()!);
    public bool? IsRestricted => bool.Parse(_jsonNode["is_restricted"]?.ToString()!);

    public int VolumePercent => int.Parse(_jsonNode["volume_percent"]?.ToString()!);

    public SpotifyDevice(JsonNode jsonNode)
    {
        _jsonNode = jsonNode;
    }
}