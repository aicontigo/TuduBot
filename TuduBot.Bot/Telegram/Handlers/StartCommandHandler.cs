using Telegram.Bot;
using Telegram.Bot.Types;
using TuduBot.Application.Interfaces;

namespace TuduBot.Bot.Telegram.Handlers;

public class StartCommandHandler : ICommandHandler
{
    public string Command => "start";

    private readonly IStartHandler _handler;
    private readonly ITelegramBotClient _bot;

    public StartCommandHandler(IStartHandler handler, ITelegramBotClient bot)
    {
        _handler = handler;
        _bot = bot;
    }

    public async Task Handle(Update update, CancellationToken cancellationToken)
    {
        var message = update.Message;
        if (message?.From == null)
            return;

        var userId = message.From.Id;

        await _handler.Handle(userId, cancellationToken);

        await _bot.SendMessage(
            chatId: userId,
            text: "👋 Hi! I'm — TuduBot. I'll help you work with Todoist directly from Telegram.",
            cancellationToken: cancellationToken
        );
    }
}
