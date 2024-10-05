using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SkillUpHub.Profile.Infrastructure.Interfaces;

namespace SkillUpHub.Profile.Infrastructure.Services;

public class RabbitMqListenerService(
    IMessageBusClient messageBusClient, 
    IServiceProvider serviceProvider, 
    IRabbitMqMessageHandler rabbitMqMessageHandler) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        messageBusClient.Subscribe<Guid>("createUser", HandleCreateUserMessage);
        return Task.CompletedTask;
    }

    private async void HandleCreateUserMessage(Guid userId)
    {
        using var scope = serviceProvider.CreateScope();
        await rabbitMqMessageHandler.CreateDefaultUserProfileAsync(userId);
    }
}