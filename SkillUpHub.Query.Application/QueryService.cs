using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SkillUpHub.Query.Application;

public static class QueryService
{
    public static IServiceCollection AddQueryServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDbConnection>(sp => new Npgsql.NpgsqlConnection(configuration.GetConnectionString("DefaultConnection")));
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(QueryService).Assembly);
        });
        
        return services;
    }
}