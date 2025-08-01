using Telegram.Bot.Types;

namespace TuduBot.Bot.Telegram.Handlers;

public interface ICommandHandler
{
    string Command { get; }

    Task Handle(Update update, CancellationToken cancellationToken);
}
