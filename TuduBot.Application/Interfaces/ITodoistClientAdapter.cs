using Todoist.Net.Models;

public interface ITodoistClientAdapter
{
    Task<IEnumerable<Project>> GetProjects(string token, CancellationToken cancellationToken);
    Task AddTask(string token, string content, string projectId, CancellationToken cancellationToken);
}
