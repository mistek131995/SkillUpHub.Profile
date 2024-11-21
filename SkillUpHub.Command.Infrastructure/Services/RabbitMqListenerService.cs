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
        messageBusClient.Subscribe("createUser", async (CreateUserMessage message) => await HandleCreateUserMessage(message.UserId, message.SessionId));
        return Task.CompletedTask;
    }

    private async Task HandleCreateUserMessage(Guid userId, Guid sessionId)
    {
        using var scope = serviceProvider.CreateScope();
        var rabbitMqMessageHandler = scope.ServiceProvider.GetService<IRabbitMqMessageHandler>()!;
        await rabbitMqMessageHandler.CreateDefaultUserProfileAsync(userId, sessionId);
    }
}