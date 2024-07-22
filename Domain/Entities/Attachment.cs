using Domain.Enums;

namespace Domain.Entities;

public class Attachment
{
    public Guid Id { get; set; }
    public required string OriginalName { get; set; }
    public required string Extension { get; set; }
    public required string Path { get; set; }
    public required DateTime CreateData { get; set; }
    public bool IsDeleted { get; set; }
    public AttachmentType Type { get; set; }
}
