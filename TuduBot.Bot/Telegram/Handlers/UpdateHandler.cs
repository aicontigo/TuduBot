using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TuduBot.Bot.Telegram.Handlers;
using TuduBot.Application.Interfaces;
using TuduBot.Application.Models;

namespace TuduBot.Bot.Telegram;

public class UpdateHandler
{
    private readonly IEnumerable<ICommandHandler> _commandHandlers;
    private readonly CallbackHandler _callbackHandler;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ITelegramBotClient _bot;

    public UpdateHandler(
        IEnumerable<ICommandHandler> commandHandlers,
        CallbackHandler callbackHandler,
        IServiceScopeFactory scopeFactory,
        ITelegramBotClient bot)
    {
        _commandHandlers = commandHandlers;
        _callbackHandler = callbackHandler;
        _scopeFactory = scopeFactory;
        _bot = bot;
    }

    public async Task Handle(Update update, CancellationToken cancellationToken)
    {
        // handle callback-buttons (inline)
        if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery != null)
        {
            await _callbackHandler.Handle(update.CallbackQuery, cancellationToken);
            return;
        }

        // handle text commands (/start, /add, ...)
        if (update.Type == UpdateType.Message &&
            update.Message != null &&
            !string.IsNullOrWhiteSpace(update.Message.Text) &&
            update.Message.Text.StartsWith("/"))
        {
            var commandText = update.Message.Text.Split(' ')[0].TrimStart('/');
            var handler = _commandHandlers.FirstOrDefault(h =>
                string.Equals(h.Command, commandText, StringComparison.OrdinalIgnoreCase));

            if (handler != null)
            {
                await handler.Handle(update, cancellationToken);
                return;
            }
        }

        // handle forwarded messages
        if (update.Message?.ForwardFrom != null || update.Message?.ForwardFromChat != null)
        {
            var forwardedText = update.Message.Text;
            if (!string.IsNullOrWhiteSpace(forwardedText) && update.Message.From != null)
            {
                var userId = update.Message.From.Id;

                using var scope = _scopeFactory.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IAddTaskHandler>();

                var request = new AddTaskRequest
                {
                    TelegramUserId = userId,
                    Text = forwardedText
                };

                try
                {
                    await handler.Handle(request, cancellationToken);
                    await _bot.SendMessage(
                        update.Message.Chat.Id,
                        "Task has been added from forwarded message!",
                        cancellationToken: cancellationToken);
                }
                catch (Exception ex)
                {
                    await _bot.SendMessage(
                        update.Message.Chat.Id,
                        $"Error: {ex.Message}",
                        cancellationToken: cancellationToken);
                }
            }
        }
    }
}
