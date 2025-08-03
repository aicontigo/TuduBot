using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TuduBot.Application.Interfaces;

namespace TuduBot.Bot.Telegram.Handlers;

public class SetProjectCommandHandler : ICommandHandler
{
    public string Command => "setproject";

    private readonly IGetProjectsHandler _projects;
    private readonly ITelegramBotClient _bot;

    public SetProjectCommandHandler(IGetProjectsHandler projects, ITelegramBotClient bot)
    {
        _projects = projects;
        _bot = bot;
    }

    public async Task Handle(Update update, CancellationToken cancellationToken)
    {
        var message = update.Message;
        if (message?.From == null)
            return;

        var userId = message.From.Id;
        var chatId = message.Chat.Id;

        try
        {
            var projects = await _projects.Handle(userId, cancellationToken);
            if (projects.Count == 0)
            {
                await _bot.SendMessage(chatId, "Projects not found.", cancellationToken: cancellationToken);
                return;
            }

            var keyboard = new InlineKeyboardMarkup(
                projects.Select(p =>
                    InlineKeyboardButton.WithCallbackData(p.Name, $"set_project:{p.Id}")));

            await _bot.SendMessage(chatId, "Chose default project:", replyMarkup: keyboard, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            await _bot.SendMessage(chatId, $"Error: {ex.Message}", cancellationToken: cancellationToken);
        }
    }
}
