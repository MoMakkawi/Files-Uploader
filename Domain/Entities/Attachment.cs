using Domain.Enums;

namespace Domain.Entities;

public class Attachment
{
    public Guid Id { get; set; }
    public required string OriginalName { get; set; }
    public required string UniqueName { get; set; }
    public required string Extension { get; set; }
    public required string Path { get; set; }
    public DateTime CreateData { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
    public AttachmentType Type { get; set; }
}
