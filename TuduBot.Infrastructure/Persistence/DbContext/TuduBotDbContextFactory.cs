using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TuduBot.Infrastructure.Persistence;

public class TuduBotDbContextFactory : IDesignTimeDbContextFactory<TuduBotDbContext>
{
    public TuduBotDbContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../TuduBot.Bot");

        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<TuduBotDbContext>();
        optionsBuilder
        .UseNpgsql(config.GetConnectionString("Postgres"))
        .UseSnakeCaseNamingConvention();

        return new TuduBotDbContext(optionsBuilder.Options);
    }
}
