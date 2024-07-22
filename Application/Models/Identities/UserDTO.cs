namespace Application.Models.Identities;

public class UserDTO
{
    public Guid Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
}
