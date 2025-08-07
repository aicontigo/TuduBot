using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TuduBot.Bot.Telegram;

public class BotCommandRegistrar
{
    private readonly ITelegramBotClient _bot;

    public BotCommandRegistrar(ITelegramBotClient bot)
    {
        _bot = bot;
    }

    public async Task RegisterCommands(CancellationToken cancellationToken = default)
    {
        await _bot.SetMyCommands(BotCommandList.Commands, new BotCommandScopeDefault(), cancellationToken: cancellationToken);
    }
}
