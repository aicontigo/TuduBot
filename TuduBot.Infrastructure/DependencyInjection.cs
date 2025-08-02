using Microsoft.Extensions.DependencyInjection;
using TuduBot.Application.Interfaces;
using TuduBot.Infrastructure.Repositories;
using TuduBot.Infrastructure.Security;

namespace TuduBot.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICryptoService, CryptoService>();
        return services;
        RandomNumberGenerator.Seed = new Random().Next();
    }
}
