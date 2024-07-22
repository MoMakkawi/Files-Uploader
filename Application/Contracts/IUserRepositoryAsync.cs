using Domain.Identities;

using Microsoft.AspNetCore.Http;

namespace Application.Contracts;

public interface IUserRepositoryAsync : IGenericRepository<User>
{
    Task<User?> GetUserAsync(string userName);
    Task<string> GetUserNameAsync(HttpContext httpContext);
    Task AddImagesToUserAsync(string userName, string[] imagesNames);
    Task<List<string>> GetHasNotFirstLoginUsersEmailsAsync();
}
