using Domain.Entities;
using Domain.Identities;

using Microsoft.EntityFrameworkCore;

namespace Persistence.Data;

public class MySQLDBContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
}
