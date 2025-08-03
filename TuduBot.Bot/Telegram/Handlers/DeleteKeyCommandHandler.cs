using Telegram.Bot;
using Telegram.Bot.Types;
using TuduBot.Application.Interfaces;

namespace TuduBot.Bot.Telegram.Handlers;

public class DeleteKeyCommandHandler : ICommandHandler
{
    public string Command => "deletekey";

    private readonly IDeleteApiKeyHandler _handler;
    private readonly ITelegramBotClient _bot;

    public DeleteKeyCommandHandler(IDeleteApiKeyHandler handler, ITelegramBotClient bot)
    {
        _handler = handler;
        _bot = bot;
    }

    public async Task Handle(Update update, CancellationToken cancellationToken)
    {
        var message = update.Message;
        if (message?.From == null)
            return;

        try
        {
            await _handler.Handle(message.From.Id, cancellationToken);
            await _bot.SendMessage(message.Chat.Id, "Key and project have been deleted", cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            await _bot.SendMessage(message.Chat.Id, $"Error: {ex.Message}", cancellationToken: cancellationToken);
        }
    }
}
