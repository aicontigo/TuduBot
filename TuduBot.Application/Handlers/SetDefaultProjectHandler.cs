using TuduBot.Application.Interfaces;

namespace TuduBot.Application.Handlers;

public class SetDefaultProjectHandler : ISetDefaultProjectHandler
{
    private readonly IUserRepository _users;

    public SetDefaultProjectHandler(IUserRepository users)
    {
        _users = users;
    }

    public async Task Handle(long telegramUserId, string projectId, CancellationToken cancellationToken)
    {
        var user = await _users.GetByTelegramId(telegramUserId, cancellationToken)
                   ?? throw new InvalidOperationException("User not found. Please set your API key first using /setkey command.");

        user.DefaultProjectId = projectId;
        user.UpdatedAt = DateTime.UtcNow;

        await _users.Update(user, cancellationToken);
    }
}
