using Microsoft.EntityFrameworkCore;
using TuduBot.Application;
using TuduBot.Bot;
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

var host = builder.Build();
host.Run();
