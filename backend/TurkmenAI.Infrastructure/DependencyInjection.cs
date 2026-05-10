using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TurkmenAI.Domain.Ai;
using TurkmenAI.Infrastructure.Ai;
using TurkmenAI.Infrastructure.Persistence;

namespace TurkmenAI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration config)
    {
        // 1) DB
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        // 2) AI Provider — config'e göre seçilir. Bu sayede provider değişir, kod değişmez.
        var providerName = config["Ai:Provider"] ?? "mock";
        switch (providerName.ToLowerInvariant())
        {
            case "groq":
                services.Configure<GroqOptions>(config.GetSection("Ai:Groq"));
                services.AddHttpClient<IAiProvider, GroqAiProvider>();
                break;
            case "mock":
            default:
                services.AddScoped<IAiProvider, MockAiProvider>();
                break;
        }

        // 3) RAG
        services.AddScoped<IRagService, SqlRagService>();

        return services;
    }
}
