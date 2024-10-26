using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkillUpHub.Command.Application.Handlers;
using SkillUpHub.Command.Infrastructure.Clients;
using SkillUpHub.Command.Infrastructure.Contexts;
using SkillUpHub.Command.Infrastructure.Interfaces;
using SkillUpHub.Command.Infrastructure.Providers;
using SkillUpHub.Command.Infrastructure.Services;
using SkillUpHub.Profile.Contract.Providers;

namespace SkillUpHub.Command.Application;

public static class CommandService
{
    public static IServiceCollection AddCommands(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PGContext>(option => 
            option.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CommandService).Assembly);
        });
        
        services.AddScoped<IRepositoryProvider, RepositoryProvider>();

        services.AddSingleton<IMessageBusClient, RabbitMqClient>(x =>
            new RabbitMqClient(configuration.GetSection("RabbitMqHost").Value!));
        services.AddScoped<IRabbitMqMessageHandler, RabbitMqMessageHandler>();

        services.AddHostedService<RabbitMqListenerService>();
        
        return services;
    }
}