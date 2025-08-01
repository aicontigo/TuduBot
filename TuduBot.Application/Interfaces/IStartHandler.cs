namespace TuduBot.Application.Interfaces;

public interface IStartHandler
{
    Task Handle(long telegramUserId, CancellationToken cancellationToken);
}
