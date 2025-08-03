using Microsoft.Extensions.DependencyInjection;
using TuduBot.Application.Handlers;
using TuduBot.Application.Interfaces;

namespace TuduBot.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IStartHandler, StartHandler>();
        services.AddScoped<ISetApiKeyHandler, SetApiKeyHandler>();
        services.AddScoped<IGetProjectsHandler, GetProjectsHandler>();
        services.AddScoped<ISetDefaultProjectHandler, SetDefaultProjectHandler>();

        return services;
    }
}
