using Microsoft.EntityFrameworkCore;
using TuduBot.Application.Interfaces;
using TuduBot.Domain.Entities;
using TuduBot.Infrastructure.Persistence;

namespace TuduBot.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TuduBotDbContext _context;

    public UserRepository(TuduBotDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByTelegramId(long telegramUserId, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.TelegramUserId == telegramUserId, cancellationToken);
    }

    public async Task<User> Add(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task Update(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
