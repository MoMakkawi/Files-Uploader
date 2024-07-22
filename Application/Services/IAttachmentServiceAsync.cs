using Domain.Entities;

using Microsoft.AspNetCore.Http;

namespace Application.Services;

internal interface IAttachmentServiceAsync
{
    Task<Attachment[]> SaveAsync(string username, IFormFileCollection? attachmentFiles);
    Task<bool> DeleteAsync(string username, IFormFile attachmentFile);
    Task<Attachment[]> GetUserAttachmentsAsync(string userName);
    Task<string> GetAsBase64Async(string path);
}
