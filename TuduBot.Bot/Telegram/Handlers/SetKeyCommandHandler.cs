using Telegram.Bot;
using Telegram.Bot.Types;
using TuduBot.Application.Interfaces;
using Telegram.Bot.Types.Enums;

namespace TuduBot.Bot.Telegram.Handlers;

public class SetKeyCommandHandler : ICommandHandler
{
    public string Command => "setkey";

    private readonly ISetApiKeyHandler _handler;
    private readonly ITelegramBotClient _bot;

    public SetKeyCommandHandler(ISetApiKeyHandler handler, ITelegramBotClient bot)
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
            await _bot.SendMessage(message.Chat.Id,
                "Send key in such format:\n`/setkey <your_api_key>`",
                parseMode: ParseMode.Markdown,
                cancellationToken: cancellationToken);
            return;
        }

        var apiKey = args[1].Trim();

        try
        {
            await _handler.Handle(message.From.Id, apiKey, cancellationToken);
            await _bot.SendMessage(message.Chat.Id,
                "Key is saved successfully!",
                cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            await _bot.SendMessage(message.Chat.Id,
                $"Error: {ex.Message}",
                cancellationToken: cancellationToken);
        }
    }
}
