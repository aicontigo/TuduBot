using Telegram.Bot;
using Telegram.Bot.Types;
using TuduBot.Application.Interfaces;

namespace TuduBot.Bot.Telegram.Handlers;

public class CallbackHandler
{
    private readonly ISetDefaultProjectHandler _handler;
    private readonly ITelegramBotClient _bot;
    private readonly IServiceScopeFactory _scopeFactory;

    public CallbackHandler(ISetDefaultProjectHandler handler,
        ITelegramBotClient bot,
        IServiceScopeFactory scopeFactory)
    {
        _handler = handler;
        _bot = bot;
        _scopeFactory = scopeFactory;
    }

    public async Task Handle(CallbackQuery callback, CancellationToken cancellationToken)
    {
        if (callback == null || callback.From == null || callback.Message == null)
            return;

        if (callback.Data?.StartsWith("menu:/") == true)
        {
            var command = callback.Data.Replace("menu:", string.Empty);

            await _bot.AnswerCallbackQuery(callback.Id, cancellationToken: cancellationToken);

            // Эмулируем команду как обычный текст
            var update = new Update
            {
                Message = new Message
                {
                    From = callback.From,
                    Chat = callback.Message!.Chat,
                    Text = command
                }
            };

            using var scope = _scopeFactory.CreateScope();
            var updateHandler = scope.ServiceProvider.GetRequiredService<UpdateHandler>();
            await updateHandler.Handle(update, cancellationToken);
            return;
        }



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
