using TuduBot.Application.Interfaces;
using TuduBot.Application.Models;
using TuduBot.Domain.Entities;
using Todoist.Net;
using Todoist.Net.Models;

namespace TuduBot.Application.Handlers;

public class AddTaskHandler : IAddTaskHandler
{
    private readonly IUserRepository _users;
    private readonly ICryptoService _crypto;
    private readonly ITodoistClientAdapter _todoist;

    public AddTaskHandler(IUserRepository users, ICryptoService crypto, ITodoistClientAdapter todoist)
    {
        _users = users;
        _crypto = crypto;
        _todoist = todoist;
    }

    public async Task Handle(AddTaskRequest request, CancellationToken cancellationToken)
    {
        var user = await _users.GetByTelegramId(request.TelegramUserId, cancellationToken)
                   ?? throw new InvalidOperationException("Пользователь не найден");

        if (string.IsNullOrWhiteSpace(user.TodoistApiKey))
            throw new InvalidOperationException("Сначала установи API-ключ через /setkey");

        if (string.IsNullOrWhiteSpace(user.DefaultProjectId))
            throw new InvalidOperationException("Сначала выбери проект через /setproject");

        var token = _crypto.Decrypt(user.TodoistApiKey);

        await _todoist.AddTask(token, request.Text, user.DefaultProjectId, cancellationToken);
    }
}
