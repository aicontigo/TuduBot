using TuduBot.Application.Interfaces;

namespace TuduBot.Application.Handlers;

public class DeleteApiKeyHandler : IDeleteApiKeyHandler
{
    private readonly IUserRepository _users;

    public DeleteApiKeyHandler(IUserRepository users)
    {
        _users = users;
    }

    public async Task Handle(long telegramUserId, CancellationToken cancellationToken)
    {
        var user = await _users.GetByTelegramId(telegramUserId, cancellationToken)
                   ?? throw new InvalidOperationException("User not found");

        user.TodoistApiKey = null;
        user.DefaultProjectId = null;
        user.UpdatedAt = DateTime.UtcNow;

        await _users.Update(user, cancellationToken);
    }
}
