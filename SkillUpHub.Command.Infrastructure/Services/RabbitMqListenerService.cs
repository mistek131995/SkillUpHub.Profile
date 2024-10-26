using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SkillUpHub.Command.Infrastructure.Interfaces;

namespace SkillUpHub.Command.Infrastructure.Services;

public class RabbitMqListenerService(
    IMessageBusClient messageBusClient, 
    IServiceProvider serviceProvider) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        messageBusClient.Subscribe<Guid>("createUser", HandleCreateUserMessage);
        return Task.CompletedTask;
    }

    private async void HandleCreateUserMessage(Guid userId)
    {
        using var scope = serviceProvider.CreateScope();
        var rabbitMqMessageHandler = scope.ServiceProvider.GetService<IRabbitMqMessageHandler>()!;
        await rabbitMqMessageHandler.CreateDefaultUserProfileAsync(userId);
    }
}