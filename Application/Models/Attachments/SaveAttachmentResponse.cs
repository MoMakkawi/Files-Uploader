using Domain.Enums;

namespace Application.Models.Attachments;

public record SaveAttachmentResponse(
    string OriginalName,
    string UniqueName,
    string Extension,
    string Path,
    DateTime CreateData,
    AttachmentType Type);

