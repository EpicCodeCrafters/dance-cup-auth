using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ECC.DanceCup.Auth.Application.Abstractions.Security;
using ECC.DanceCup.Auth.Domain.Model;
using ECC.DanceCup.Auth.Infrastructure.Security.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ECC.DanceCup.Auth.Infrastructure.Security.Services;

public class TokenProvider : ITokenProvider
{
    private readonly IOptions<SecurityOptions> _securityOptions;

    public TokenProvider(IOptions<SecurityOptions> securityOptions)
    {
        _securityOptions = securityOptions;
    }

    public string CreateUserToken(User user)
    {
        var claims = new Claim[]
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username)
        };

        var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_securityOptions.Value.Secret));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            issuer: _securityOptions.Value.Issuer,  
            audience: _securityOptions.Value.Audience, 
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddMinutes(_securityOptions.Value.TokenExpiresMinutes)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}