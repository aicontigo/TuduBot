using Telegram.Bot;
using Telegram.Bot.Types;
using TuduBot.Application.Interfaces;

namespace TuduBot.Bot.Telegram.Handlers;

public class CallbackHandler
{
    private readonly ISetDefaultProjectHandler _handler;
    private readonly ITelegramBotClient _bot;

    public CallbackHandler(ISetDefaultProjectHandler handler, ITelegramBotClient bot)
    {
        _handler = handler;
        _bot = bot;
    }

    public async Task Handle(CallbackQuery callback, CancellationToken cancellationToken)
    {
        if (callback == null || callback.From == null || callback.Message == null)
            return;
            
        if (callback.Data?.StartsWith("set_project:") != true)
            return;

        var projectId = callback.Data.Split(':')[1];
        var userId = callback.From.Id;

        try
        {
            await _handler.Handle(userId, projectId, cancellationToken);
            await _bot.AnswerCallbackQuery(callback.Id, "Project is chosen!");
            await _bot.SendMessage(callback.Message.Chat.Id, "Project has been set as default.", cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            await _bot.AnswerCallbackQuery(callback.Id, "Error setting project.");
            await _bot.SendMessage(callback.Message.Chat.Id, $"Error: {ex.Message}", cancellationToken: cancellationToken);
        }
    }
}
