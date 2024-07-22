using Application.Models.Attachments;
using Domain.Identities;

using Microsoft.AspNetCore.Http;

namespace Application.Services;

public interface IAttachmentServiceAsync
{
    Task<IEnumerable<SavedAttachmentResponse>> SaveAsync(User user, IFormFileCollection? attachmentFiles);
    Task<bool> DeleteAsync(string userName, IFormFile attachmentFile);
    Task<string> GetAsBase64Async(string path);
}
