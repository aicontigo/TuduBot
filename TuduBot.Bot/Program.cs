using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using TuduBot.Application;
using TuduBot.Bot;
using TuduBot.Bot.Telegram;
using TuduBot.Bot.Telegram.Handlers;
using TuduBot.Infrastructure;
using TuduBot.Infrastructure.Persistence;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddDbContext<TuduBotDbContext>(options =>
    options
    .UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
    .UseSnakeCaseNamingConvention());

builder.Services.AddInfrastructure();
builder.Services.AddApplication();

builder.Services.AddSingleton<ITelegramBotClient>(_ =>
    new TelegramBotClient(builder.Configuration["Telegram:BotToken"]));

builder.Services.AddHostedService<TelegramBotService>();
builder.Services.AddScoped<ICommandHandler, StartCommandHandler>();
builder.Services.AddScoped<UpdateHandler>();



var host = builder.Build();
host.Run();
