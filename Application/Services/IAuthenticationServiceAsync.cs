using Application.Models.Security;
using Domain.Security;

namespace Application.Services;

public interface IAuthenticationServiceAsync
{
    TokenData GenerateToken(string userName);
    TokenData GenerateRefreshToken(string accessToken);
    bool TryToGetValidRefreshTokenData(RefreshTokenRequest request, out TokenData? refreshTokenData);
}
