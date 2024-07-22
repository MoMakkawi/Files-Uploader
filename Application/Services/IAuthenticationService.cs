using Application.Models;

using Domain.Security;

namespace Application.Services;

internal interface IAuthenticationService
{
    TokenData GenerateToken(string userName);
    TokenData GenerateRefreshToken(string accessToken);
    bool TryToGetValidRefreshTokenData(RefreshTokenRequest request, out TokenData? refreshTokenData);
}
