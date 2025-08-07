using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TuduBot.Application.Interfaces;

namespace TuduBot.Bot.Telegram.Handlers;

public class MenuCommandHandler : ICommandHandler
{
    public string Command => "menu";

    private readonly ITelegramBotClient _bot;

    public MenuCommandHandler(ITelegramBotClient bot)
    {
        _bot = bot;
    }

    public async Task Handle(Update update, CancellationToken cancellationToken)
    {
        var message = update.Message;
        if (message?.From == null)
            return;

        var keyboard = new InlineKeyboardMarkup(new[]
        {
            new[] {
                InlineKeyboardButton.WithCallbackData("🔑 Set key", "menu:/setkey"),
                InlineKeyboardButton.WithCallbackData("❌ Delete key", "menu:/deletekey")
            },
            new[] {
                InlineKeyboardButton.WithCallbackData("📋 Choose preject", "menu:/setproject"),
                InlineKeyboardButton.WithCallbackData("📄 My projects", "menu:/projects")
            },
            new[] {
                InlineKeyboardButton.WithCallbackData("➕ Add task", "menu:/add"),
                InlineKeyboardButton.WithCallbackData("ℹ️ Help", "menu:/help")
            }
        });

        await _bot.SendMessage(
            chatId: message.Chat.Id,
            text: "📋 Main menu:",
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);
    }
}
