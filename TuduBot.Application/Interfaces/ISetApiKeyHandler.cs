
namespace TuduBot.Application.Interfaces;

public interface ISetApiKeyHandler
{
    Task Handle(long telegramUserId, string apiKey, CancellationToken cancellationToken);
}