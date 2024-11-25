using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SkillUpHub.Command.Infrastructure.Interfaces;

namespace SkillUpHub.Command.Infrastructure.Services;

public class RabbitMqListenerService(
    IMessageBusClient messageBusClient, 
    IServiceProvider serviceProvider) : BackgroundService
{
    private record CreateUserMessage(Guid UserId, Guid SessionId);
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceProvider.CreateScope();
        var rabbitMqMessageHandler = scope.ServiceProvider.GetService<IRabbitMqMessageHandler>()!;
        
        messageBusClient.Subscribe("create-account-complete", async (CreateUserMessage message) 
            => await rabbitMqMessageHandler.CreateDefaultUserProfileAsync(message.UserId, message.SessionId));
        
        return Task.CompletedTask;
    }
}