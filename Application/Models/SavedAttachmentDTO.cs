using Domain.Enums;

namespace Application.Models;

public record SavedAttachmentResponse(
    string OriginalName,
    string Extension,
    string Path,
    DateTime CreateData,
    AttachmentType Type);

