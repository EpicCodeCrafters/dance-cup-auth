namespace ECC.DanceCup.Auth.Infrastructure.Security.Options;

public class SecurityOptions
{
    public string Secret { get; set; } = string.Empty;
    
    public int TokenExpiresMinutes { get; set; }
}