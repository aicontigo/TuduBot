using TuduBot.Application.Interfaces;
using TuduBot.Domain.Entities;

namespace TuduBot.Application.Handlers;
public class StartHandler : IStartHandler
{
    private readonly IUserRepository _users;

    public StartHandler(IUserRepository users) => _users = users;

    public async Task Handle(long telegramUserId, CancellationToken cancellationToken)
    {
        var user = await _users.GetByTelegramId(telegramUserId, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();

        if (user != null) return;

        await _users.Add(new User
        {
            Id = Guid.NewGuid(),
            TelegramUserId = telegramUserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }, cancellationToken);
    }
}
