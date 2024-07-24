using Domain.Entities;
using Domain.Identities;

using Microsoft.AspNetCore.Http;

namespace Application.Contracts;

public interface IUserRepositoryAsync : IGenericRepositoryAsync<User>
{
    Task<User?> GetByUserNameAsync(string userName);
    Task<string> GetUserNameAsync(HttpContext httpContext);
    Task<User> GetLoginUserAsync(HttpContext httpContext);
    Task<List<string>> GetHasNotFirstLoginUsersEmailsAsync();
}
