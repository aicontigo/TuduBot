namespace TuduBot.Application.Interfaces;

public interface IDeleteApiKeyHandler
{
    Task Handle(long telegramUserId, CancellationToken cancellationToken);
}
