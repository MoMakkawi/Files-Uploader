using Domain.Enums;

using Microsoft.AspNetCore.Http;

namespace Application.Models.Attachments;

public record SaveAttachmentCommand(AttachmentType Type, IFormFile Attachment);
