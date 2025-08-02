using Todoist.Net;
using Todoist.Net.Models;
using TuduBot.Application.Interfaces;

namespace TuduBot.Infrastructure.Todoist;

public class TodoistClientAdapter : ITodoistClientAdapter
{
    public async Task<IEnumerable<Project>> GetProjects(string token, CancellationToken cancellationToken)
    {
        var client = new TodoistClient(token);
        return await client.Projects.GetAsync(cancellationToken);
    }
}
