namespace SkillUpHub.Command.Infrastructure.Interfaces;

public interface IRabbitMqMessageHandler
{
    Task CreateDefaultUserProfileAsync(Guid userId, Guid sessionId);
}