using Microsoft.Extensions.DependencyInjection;
using TurkmenAI.Application.Modules;

namespace TurkmenAI.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<AssistantService>();
        return services;
    }
}
