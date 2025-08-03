using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TuduBot.Application.Interfaces;
using TuduBot.Application.Models;

namespace TuduBot.Bot.Telegram.Handlers;

public class AddCommandHandler : ICommandHandler
{
    public string Command => "add";

    private readonly IAddTaskHandler _handler;
    private readonly ITelegramBotClient _bot;

    public AddCommandHandler(IAddTaskHandler handler, ITelegramBotClient bot)
    {
        _handler = handler;
        _bot = bot;
    }

    public async Task Handle(Update update, CancellationToken cancellationToken)
    {
        var message = update.Message;
        if (message?.From == null || string.IsNullOrWhiteSpace(message.Text))
            return;

        var args = message.Text.Split(' ', 2);
        if (args.Length < 2)
        {
            await _bot.SendMessage(message.Chat.Id, "Add task like this:\n`/add <task content>`", parseMode: ParseMode.Markdown, cancellationToken: cancellationToken);
            return;
        }

        var text = args[1].Trim();

        try
        {
            await _handler.Handle(new AddTaskRequest
            {
                TelegramUserId = message.From.Id,
                Text = text
            }, cancellationToken);

            await _bot.SendMessage(message.Chat.Id, "Task has been added!", cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            await _bot.SendMessage(message.Chat.Id, $"Error: {ex.Message}", cancellationToken: cancellationToken);
        }
    }
}
