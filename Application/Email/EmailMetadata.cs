namespace Application.Email;
public sealed record EmailMetadata(
    string ToAddress,
    string Subject,
    string? Body = "");