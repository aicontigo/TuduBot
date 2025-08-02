using TuduBot.Application.Interfaces;
using TuduBot.Domain.Entities;
using Todoist.Net;
using Todoist.Net.Models;

namespace TuduBot.Application.Handlers;

public class SetApiKeyHandler : ISetApiKeyHandler
{
    private readonly IUserRepository _users;
    private readonly ICryptoService _crypto;

    public SetApiKeyHandler(IUserRepository users, ICryptoService crypto)
    {
        _users = users;
        _crypto = crypto;
    }

    public async Task Handle(long telegramUserId, string apiKey, CancellationToken cancellationToken)
    {
        // Check key via Todoist API
        var client = new TodoistClient(apiKey);
        var userInfo = await client.Users.GetCurrentAsync(cancellationToken);
        if (userInfo == null)
            throw new InvalidOperationException("Invalid API-key provided.");

        var user = await _users.GetByTelegramId(telegramUserId, cancellationToken);
        if (user == null)
        {
            user = new TuduBot.Domain.Entities.User
            {
                Id = Guid.NewGuid(),
                TelegramUserId = telegramUserId,
                CreatedAt = DateTime.UtcNow
            };
            await _users.Add(user, cancellationToken);
        }

        user.TodoistApiKey = _crypto.Encrypt(apiKey);
        user.UpdatedAt = DateTime.UtcNow;

        await _users.Update(user, cancellationToken);
    }
}
