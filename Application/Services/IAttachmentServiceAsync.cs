using Application.Models.Attachments;

using Domain.Entities;
using Domain.Identities;

namespace Application.Services;

public interface IAttachmentServiceAsync
{
    Task<IEnumerable<Attachment>> SaveAsync(User user, List<SaveAttachmentCommand>? attachmentFiles);
    Task DeleteAsync(User user, string attachmentOriginalName);
    Task<string?> GetAsBase64Async(string attachmentUniqueName);
}
