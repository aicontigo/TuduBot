using TuduBot.Application.Interfaces;
using TuduBot.Application.Models;
using Todoist.Net;
using TuduBot.Domain.Entities;

namespace TuduBot.Application.Handlers;

public class GetProjectsHandler : IGetProjectsHandler
{
    private readonly IUserRepository _users;
    private readonly ICryptoService _crypto;
    private readonly ITodoistClientAdapter _todoist;

    public GetProjectsHandler(IUserRepository users, ICryptoService crypto, ITodoistClientAdapter todoist)
    {
        _users = users;
        _crypto = crypto;
        _todoist = todoist;
    }

    public async Task<IReadOnlyList<ProjectDto>> Handle(long telegramUserId, CancellationToken cancellationToken)
    {
        var user = await _users.GetByTelegramId(telegramUserId, cancellationToken);
        if (user == null || string.IsNullOrWhiteSpace(user.TodoistApiKey))
            throw new InvalidOperationException("First set API-key using /setkey");

        var decryptedKey = _crypto.Decrypt(user.TodoistApiKey);

        var client = new TodoistClient(decryptedKey);
        var projects = await _todoist.GetProjects(decryptedKey, cancellationToken);

        return projects.Select(p => new ProjectDto
        {
            Id = p.Id.ToString(),
            Name = p.Name
        }).ToList();
    }
}
