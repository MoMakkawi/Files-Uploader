using Microsoft.IdentityModel.Tokens;


using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Infrastructure.Options;
using Application.Services;
using Domain.Security;
using Application.Models.Security;

namespace Infrastructure.Services;

public class AuthenticationServiceAsync(JwtOptions jwtOptions) : IAuthenticationServiceAsync
{
    public static readonly Dictionary<string, TokenData> RefreshTokens = [];

    public TokenData GenerateToken(string userName)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Issuer = jwtOptions.Issuer,
            Audience = jwtOptions.Audience,
            Expires = DateTime.UtcNow.AddMinutes(jwtOptions.AccessTokenLifetime),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, userName)
                ])

        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(securityToken);

        return new TokenData(accessToken, tokenDescriptor.Expires.Value);
    }

    public TokenData GenerateRefreshToken(string accessToken)
    {
        var expireDate = DateTime.UtcNow.AddMonths(jwtOptions.RefreshTokenLifetime);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Issuer = jwtOptions.Issuer,
            Audience = jwtOptions.Audience,
            Expires = expireDate,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                SecurityAlgorithms.HmacSha256),
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var refreshToken = tokenHandler.WriteToken(securityToken);

        RefreshTokens[accessToken] = new TokenData(refreshToken, expireDate);

        return new TokenData(refreshToken, expireDate);
    }

    public bool TryToGetValidRefreshTokenData(RefreshTokenRequest request, [MaybeNullWhen(false)] out TokenData refreshTokenData)
    {
        var isRefreshTokenExist = RefreshTokens.TryGetValue(request.AccessToken, out TokenData existRefreshTokenData);
            refreshTokenData = existRefreshTokenData;

        return isRefreshTokenExist 
            && existRefreshTokenData!.Token == request.RefreshToken 
            && existRefreshTokenData.Expires > DateTime.UtcNow;
    }
}
