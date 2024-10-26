using System.Data;
using Microsoft.Extensions.DependencyInjection;

namespace SkillUpHub.Query.Application;

public static class QueryService
{
    public static IServiceCollection AddQueryServices(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IDbConnection>(sp => new Npgsql.NpgsqlConnection(connectionString));
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(QueryService).Assembly);
        });
        
        return services;
    }
}