using System.Text.Json.Nodes;

namespace SpotifyWrapper.SpotifyObjects;

public abstract class SpotifyItem
{
    public abstract string? Id { get; }
    public abstract string? Name { get; }
    public abstract string? Uri { get; }
    public abstract string? Href { get; }
    public abstract string? Type { get; }
    public abstract string? ReleaseDate { get; }
    
    
    public abstract int DurationMs { get; }
    public abstract bool? Explicit { get; }
    
    public abstract SpotifyExternalUrls ExternalUrls { get; }
    public abstract List<SpotifyImage>? Images { get; }


}