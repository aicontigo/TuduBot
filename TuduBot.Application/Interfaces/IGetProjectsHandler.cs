using TuduBot.Application.Models;

namespace TuduBot.Application.Interfaces;

public interface IGetProjectsHandler
{
    Task<IReadOnlyList<ProjectDto>> Handle(long telegramUserId, CancellationToken cancellationToken);
}
