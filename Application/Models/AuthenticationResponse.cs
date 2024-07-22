using Domain.Security;

namespace Application.Models;

public class AuthenticationResponse
{
    public required TokenData AccessTokenData { get; set; }
    public required TokenData RefreshTokenData { get; set; }
}