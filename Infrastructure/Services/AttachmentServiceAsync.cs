using Application.Contracts;
using Application.Helper;
using Application.Models.Attachments;
using Application.Services;

using Domain.Entities;
using Domain.Enums;
using Domain.Identities;


namespace Infrastructure.Services;

public class AttachmentServiceAsync(IUserRepositoryAsync userRepositoryAsync, IAttachmentRepositoryAsync attachmentRepositoryAsync) : IAttachmentServiceAsync
{
    private static readonly string usersAttachmentsFolderPath = Path.Combine(
      Directory.GetParent(Directory.GetCurrentDirectory()!)!.FullName,
      "UsersAttachments");

    private readonly Func<string, string> GetFullUserFolderPath = (username) => Path.Combine(usersAttachmentsFolderPath, username);

    private readonly Func<string, AttachmentType, string, string> GenerateFullAttachmentPath = (username, attachmentType, uniqueName)
        => Path.Combine(usersAttachmentsFolderPath, username, nameof(attachmentType), uniqueName);

    public async Task DeleteAsync(User user, string attachmentUniqueName)
    {
        var userAttachment = user.Attachments
            .FirstOrDefault(attachment => attachment.UniqueName == attachmentUniqueName);

        if (userAttachment is null) return;

        await attachmentRepositoryAsync.DeleteAsync(userAttachment.Id);
        File.Delete(userAttachment.Path);
    }



    public async Task<string?> GetAsBase64Async(string attachmentUniqueName)
    {
        var attachmentList = await attachmentRepositoryAsync.GetListAsync();
        var attachment = attachmentList.FirstOrDefault(attachment => attachment.UniqueName == attachmentUniqueName);
        return attachment is null ? null : AttachmentHelper.GetAsBase64(attachment!.Path);
    }


    public async Task<IEnumerable<Attachment>> SaveAsync(User user, List<SaveAttachmentCommand>? attachmentFiles)
    {
        if (attachmentFiles is null) return [];

        if (attachmentFiles.GetInvalidFilesNames() is string[] invalidFilesNames && invalidFilesNames.Length is not 0)
            throw new ArgumentException("Those files are not an images, allowed images only : " + string.Join(", ", invalidFilesNames));

        if (!Directory.Exists(GetFullUserFolderPath(user.UserName)))
            Directory.CreateDirectory(GetFullUserFolderPath(user.UserName));

        var saveTasks = attachmentFiles
            .Select(async attachmentFile => await SaveAttachmentAsync(user, attachmentFile));

        return await Task.WhenAll(saveTasks);
    }
    private async Task<Attachment> SaveAttachmentAsync(User user, SaveAttachmentCommand attachmentFile)
    {
        var extension = Path.GetExtension(attachmentFile.Attachment.FileName);
        var type = attachmentFile.Type;
        var uniqueName = Guid.NewGuid().ToString();

        var attachment = new Attachment()
        {
            OriginalName = attachmentFile.Attachment.FileName,
            UniqueName = uniqueName,
            Extension = extension,
            Type = type,
            Path = GenerateFullAttachmentPath(user.UserName, type, uniqueName)
        };

        using var stream = new FileStream(attachment.Path, FileMode.Create);
        await attachmentFile.Attachment.CopyToAsync(stream);

        user.Attachments.Add(attachment);
        await userRepositoryAsync.UpdateAsync(user);

        return attachment;
    }
}
