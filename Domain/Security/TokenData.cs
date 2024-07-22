namespace Domain.Security;

public sealed record TokenData(
    string Token,
    DateTime Expires);
