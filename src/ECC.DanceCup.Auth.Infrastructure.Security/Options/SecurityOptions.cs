namespace ECC.DanceCup.Auth.Infrastructure.Security.Options;

public class SecurityOptions
{
    public string Secret { get; set; } = string.Empty;
    
    public string Issuer { get; set; } = string.Empty;
    
    public string Audience { get; set; } = string.Empty;
    
    public int TokenExpiresMinutes { get; set; }
}