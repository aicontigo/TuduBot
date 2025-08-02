using Telegram.Bot;
using Telegram.Bot.Types;
using TuduBot.Application.Interfaces;
using Telegram.Bot.Types.Enums;

namespace TuduBot.Bot.Telegram.Handlers;

public class ProjectsCommandHandler : ICommandHandler
{
    public string Command => "projects";

    private readonly IGetProjectsHandler _handler;
    private readonly ITelegramBotClient _bot;

    public ProjectsCommandHandler(IGetProjectsHandler handler, ITelegramBotClient bot)
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
            var projects = await _handler.Handle(message.From.Id, cancellationToken);
            if (projects.Count == 0)
            {
                await _bot.SendMessage(message.Chat.Id, "You don't have any project in Todoist.", cancellationToken: cancellationToken);
                return;
            }

            var text = "*Project list:*\n" +
                       string.Join('\n', projects.Select(p => $"• `{p.Name}` (ID: `{p.Id}`)"));

            await _bot.SendMessage(
                chatId: message.Chat.Id,
                text: text,
                parseMode: ParseMode.Markdown,
                cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            await _bot.SendMessage(message.Chat.Id, $"Error: {ex.Message}", cancellationToken: cancellationToken);
        }
    }
}
