﻿using Domain.Enums;

namespace Application.Models.Attachments;

public class AttachmentDTO
{
    public required string OriginalName { get; set; }
    public required string UniqueName { get; set; }
    public required string Extension { get; set; }
    public required string Path { get; set; }
    public required DateTime CreateData { get; set; }
    public AttachmentType Type { get; set; }
}