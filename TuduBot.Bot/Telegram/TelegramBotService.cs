using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TuduBot.Bot.Telegram;

public class TelegramBotService : BackgroundService
{
    private readonly ITelegramBotClient _botClient;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TelegramBotService> _logger;

    public TelegramBotService(ITelegramBotClient botClient, IServiceScopeFactory scopeFactory, ILogger<TelegramBotService> logger)
    {
        _botClient = botClient;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }


    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new[] { UpdateType.Message }
        };

        _botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken: stoppingToken);

        _logger.LogInformation("🤖 Telegram bot started.");
        return Task.CompletedTask;
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<UpdateHandler>();
            await handler.Handle(update, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling update");
        }
    }


    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Telegram error occurred.");
        return Task.CompletedTask;
    }
}
