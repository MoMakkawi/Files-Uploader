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

    private readonly Func<string, AttachmentType, string, string, string> GenerateFullAttachmentPath = (username, attachmentType, uniqueName, extension)
        => Path.Combine(usersAttachmentsFolderPath, username, attachmentType.ToString(), uniqueName + extension);

    public async Task DeleteAsync(User user, string attachmentUniqueName)
    {
        var userAttachment = user.Attachments
            .FirstOrDefault(attachment => attachment.UniqueName == attachmentUniqueName);

        if (userAttachment is null) return;

        userAttachment.IsDeleted = true;
        await attachmentRepositoryAsync.UpdateAsync(userAttachment);

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
            throw new ArgumentException("Those files are invalid because they did not have match there attachment type only : " + string.Join(", ", invalidFilesNames));

        foreach (var attachmentType in Enum.GetValues<AttachmentType>())
            if (!Directory.Exists(Path.Combine(GetFullUserFolderPath(user.UserName), attachmentType.ToString())))
                Directory.CreateDirectory(Path.Combine(GetFullUserFolderPath(user.UserName), attachmentType.ToString()));

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
            Path = GenerateFullAttachmentPath(user.UserName, type, uniqueName, extension)
        };

        using var stream = new FileStream(attachment.Path, FileMode.Create);
        await attachmentFile.Attachment.CopyToAsync(stream);

        user.Attachments.Add(attachment);
        await userRepositoryAsync.UpdateAsync(user);

        return attachment;
    }
}
