using Infrastructure.Persistence;
using WebAPI.Infrastructure;

namespace WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddWebAPIServices(this IServiceCollection services)
    {
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddProblemDetails();
        services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>("sqlserver");
        return services;
    }
}
