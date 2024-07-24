using Domain.Entities;

namespace Domain.Identities;

public class User
{
    public Guid Id { get; set; }
    public required string UserName { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public bool HasFirstLogin { get; set; }

    public virtual ICollection<Attachment> Attachments { get; set; } = [];
}
