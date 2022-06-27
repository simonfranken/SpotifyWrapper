using System.Text.Json.Nodes;

namespace SpotifyWrapper.SpotifyObjects;

public class SpotifyAlbum
{
    private readonly JsonNode _jsonNode;

    public string? Id => _jsonNode["id"]?.ToString();
    public string? Name => _jsonNode["name"]?.ToString();
    public string? ReleaseDate => _jsonNode["release_date"]?.ToString();
    public string? ReleaseDatePrecision => _jsonNode["release_date_precision"]?.ToString();
    public string? Uri => _jsonNode["uri"]?.ToString();
    public string? Type => _jsonNode["type"]?.ToString();
    public string? AlbumType => _jsonNode["album_type"]?.ToString();
    public string? Href => _jsonNode["href"]?.ToString();
    
    public int TotalTracks => int.Parse(_jsonNode["total_tracks"]?.ToString()!);

    public SpotifyExternalUrls ExternalUrls => new SpotifyExternalUrls(_jsonNode["external_urls"]!);
    public List<SpotifyArtist>? Artists =>
        _jsonNode["artists"]?.AsArray().Select(artist => new SpotifyArtist(artist!)).ToList();
    public List<SpotifyImage>? Images =>
        _jsonNode["images"]?.AsArray().Select(image => new SpotifyImage(image!)).ToList();

    public SpotifyAlbum(JsonNode jsonNode)
    {
        _jsonNode = jsonNode;
    }
}