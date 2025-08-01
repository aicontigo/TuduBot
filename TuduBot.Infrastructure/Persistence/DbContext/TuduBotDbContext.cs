using Microsoft.EntityFrameworkCore;
using TuduBot.Domain.Entities;

namespace TuduBot.Infrastructure.Persistence;

public class TuduBotDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public TuduBotDbContext(DbContextOptions<TuduBotDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
