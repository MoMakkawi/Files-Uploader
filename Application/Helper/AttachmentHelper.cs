﻿using Application.Models.Attachments;
using Domain.Enums;

namespace Application.Helper;

public static class AttachmentHelper
{
    private static Func<SaveAttachmentCommand, bool> IsNotValidFile = (saveAttachment)
    => (saveAttachment.Type is AttachmentType.Image && !saveAttachment.Attachment.ContentType.StartsWith("image"))
    || (saveAttachment.Type is AttachmentType.Passport && !saveAttachment.Attachment.ContentType.EndsWith("pdf"))
    || saveAttachment.Type is AttachmentType.Document;

    public static string[] GetInvalidFilesNames(this List<SaveAttachmentCommand> attachmentFiles)
        => attachmentFiles
            .Where(IsNotValidFile)
            .Select(attachmentFile => attachmentFile.Attachment.FileName)
            .ToArray();

    public static string GetAsBase64(string path)
    {
        var attachmentBytes = File.ReadAllBytes(path);
        var attachmentBase64 = Convert.ToBase64String(attachmentBytes);

        return attachmentBase64;
    }
}
