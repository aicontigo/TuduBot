using Telegram.Bot.Types;
using TuduBot.Bot.Telegram.Handlers;
using Telegram.Bot.Types.Enums;

namespace TuduBot.Bot.Telegram;

public class UpdateHandler
{
    private readonly IEnumerable<ICommandHandler> _commandHandlers;

    public UpdateHandler(IEnumerable<ICommandHandler> commandHandlers)
    {
        _commandHandlers = commandHandlers;
    }

    public async Task Handle(Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message)
            return;

        var message = update.Message;
        if (message?.Text == null || !message.Text.StartsWith("/"))
            return;

        var commandText = message.Text.Split(' ')[0].TrimStart('/');

        var handler = _commandHandlers.FirstOrDefault(h =>
            string.Equals(h.Command, commandText, StringComparison.OrdinalIgnoreCase));

        if (handler != null)
        {
            await handler.Handle(update, cancellationToken);
        }
        // else  - nothing to do if no handler found
    }
}
