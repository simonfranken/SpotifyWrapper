namespace SpotifyWrapper;

public class AuthenticatorCache
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpirationDateTime { get; set; }

    public string Scope { get; set; }

    public AuthenticatorCache()
    {
        
    }
}