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
    new TelegramBotClient(builder.Configuration["Telegram:BotToken"] ?? throw new InvalidOperationException("Telegram Bot Token is not configured.")));

builder.Services.AddHostedService<TelegramBotService>();
builder.Services.AddScoped<ICommandHandler, StartCommandHandler>();
builder.Services.AddScoped<UpdateHandler>();
builder.Services.AddScoped<ICommandHandler, SetKeyCommandHandler>();
builder.Services.AddScoped<ICommandHandler, ProjectsCommandHandler>();




var host = builder.Build();
host.Run();
