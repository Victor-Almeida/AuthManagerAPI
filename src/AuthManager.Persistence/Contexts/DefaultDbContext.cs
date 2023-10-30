using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AuthManager.Domain.Identity.Entities;

namespace AuthManager.Persistence.Contexts;

public sealed class DefaultDbContext : IdentityDbContext<User, Role, Guid>
{
    public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options) { }
}
