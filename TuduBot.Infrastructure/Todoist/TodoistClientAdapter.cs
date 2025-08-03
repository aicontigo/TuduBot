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

    public async Task AddTask(string token, string content, string projectId, CancellationToken cancellationToken)
    {
        var client = new TodoistClient(token);
        var task = new AddItem(content, new ComplexId(projectId));
        await client.Items.AddAsync(task, cancellationToken);
    }

}
