namespace SkillUpHub.Profile.Infrastructure.Interfaces;

public interface IMessageBusClient
{
    void PublishMessage<T>(T message, string routingKey);
    void Subscribe<T>(string queueName, Action<T> onMessageReceived);
}