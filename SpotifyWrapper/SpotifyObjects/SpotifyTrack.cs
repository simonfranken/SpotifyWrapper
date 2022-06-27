using System.Text.Json.Nodes;

namespace SpotifyWrapper.SpotifyObjects;

public class SpotifyTrack : SpotifyItem
{
    private readonly JsonNode _jsonNode;

    public int DiscNumber => int.Parse(_jsonNode["disc_number"]?.ToString()!);
    public SpotifyAlbum Album => new(_jsonNode["album"]!);
    public List<SpotifyArtist>? Artists =>
        _jsonNode["artists"]?.AsArray().Select(node => new SpotifyArtist(node!)).ToList();
    


    public SpotifyTrack(JsonNode jsonNode)
    {
        _jsonNode = jsonNode;
    }

    public override string? Id => _jsonNode["id"]?.ToString();
    public override string? Name => _jsonNode["name"]?.ToString();
    public override string? Uri => _jsonNode["uri"]?.ToString();
    public override string? Href => _jsonNode["href"]?.ToString();
    public override string? Type => _jsonNode["type"]?.ToString();
    public override string? ReleaseDate => _jsonNode["release_date"]?.ToString();
    public override int DurationMs => int.Parse(_jsonNode["duration_ms"]?.ToString()!);
    public override bool? Explicit => bool.Parse(_jsonNode["explicit"]?.ToString()!);
    public override SpotifyExternalUrls ExternalUrls => new(_jsonNode["external_urls"]!);
    public override List<SpotifyImage>? Images =>
        _jsonNode["images"]?.AsArray().Select(node => new SpotifyImage(node!)).ToList();
}