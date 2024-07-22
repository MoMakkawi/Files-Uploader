using System.Security.Claims;

using Persistence.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Domain.Identities;
using Application.Contracts;
using Domain.Entities;

namespace Persistence.Services.Repositories;

public class UserRepositoryAsync(MySQLDBContext dbContext)
        : GenericRepository<User>(dbContext), IUserRepositoryAsync
{

    public async Task<List<string>> GetHasNotFirstLoginUsersEmailsAsync()
        => await dbContext.Users
        .Where(user => user.HasFirstLogin)
        .Select(user => user.Email)
        .ToListAsync();

    public async Task<User?> GetByUserNameAsync(string userName)
        => await dbContext.Users
        .FirstOrDefaultAsync(u => u.UserName == userName);

    public async Task<IEnumerable<Attachment>> GetUserAttachmentsAsync(string userName)
    {
        var user = await GetByUserNameAsync(userName);

        return user!.Attachments;
    }
        

    public Task<string> GetUserNameAsync(HttpContext httpContext)
    {
        var username = (httpContext.User.Identity as ClaimsIdentity)?
            .FindFirst(ClaimTypes.NameIdentifier)?
            .Value!;

        return Task.FromResult(username);
    }
}
