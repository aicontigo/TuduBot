using Todoist.Net.Models;

namespace TuduBot.Application.Interfaces;

public interface ITodoistClientAdapter
{
    Task<IEnumerable<Project>> GetProjects(string token, CancellationToken cancellationToken);
}
