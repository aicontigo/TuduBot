using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;
using TuduBot.Domain.Entities;
using TuduBot.Infrastructure.Persistence;
using TuduBot.Infrastructure.Repositories;

namespace TuduBot.Tests.Repositories;

public class UserRepositoryTests
{
    private TuduBotDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<TuduBotDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TuduBotDbContext(options);
    }

    [Fact]
    public async Task Add_should_persist_user()
    {
        var context = CreateInMemoryContext();
        var repo = new UserRepository(context);

        var user = new User
        {
            Id = Guid.NewGuid(),
            TelegramUserId = 123456789,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await repo.Add(user);

        var loaded = await context.Users.FirstOrDefaultAsync(u => u.TelegramUserId == user.TelegramUserId);

        loaded.Should().NotBeNull();
        loaded!.TelegramUserId.Should().Be(user.TelegramUserId);
    }

    [Fact]
    public async Task GetByTelegramId_should_return_correct_user()
    {
        var context = CreateInMemoryContext();
        var repo = new UserRepository(context);

        var user = new User
        {
            Id = Guid.NewGuid(),
            TelegramUserId = 987654321,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var found = await repo.GetByTelegramId(user.TelegramUserId);

        found.Should().NotBeNull();
        found!.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task Update_should_change_existing_user()
    {
        var context = CreateInMemoryContext();
        var repo = new UserRepository(context);

        var user = new User
        {
            Id = Guid.NewGuid(),
            TelegramUserId = 111222333,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            TodoistApiKey = null
        };

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        user.TodoistApiKey = "new-key";
        await repo.Update(user);

        var updated = await context.Users.FindAsync(user.Id);
        updated!.TodoistApiKey.Should().Be("new-key");
    }

    [Fact]
    public async Task Delete_should_remove_user()
    {
        var context = CreateInMemoryContext();
        var repo = new UserRepository(context);

        var user = new User
        {
            Id = Guid.NewGuid(),
            TelegramUserId = 444555666,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        await repo.Delete(user);

        var exists = await context.Users.AnyAsync(u => u.Id == user.Id);
        exists.Should().BeFalse();
    }
}
