using Microsoft.Extensions.DependencyInjection;
using TuduBot.Application.Interfaces;
using TuduBot.Infrastructure.Repositories;

namespace TuduBot.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}
