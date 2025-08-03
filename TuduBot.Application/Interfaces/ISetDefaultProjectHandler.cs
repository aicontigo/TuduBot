namespace TuduBot.Application.Interfaces;

public interface ISetDefaultProjectHandler
{
    Task Handle(long telegramUserId, string projectId, CancellationToken cancellationToken);
}
