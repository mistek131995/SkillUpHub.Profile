namespace SkillUpHub.Command.Infrastructure.Interfaces;

public interface IMessageBusClient
{
    void Initialize();
    void PublishMessage<T>(T message, string routingKey);
    void PublishErrorMessage(Exception exception);
    void Subscribe<T>(string queueName, Action<T> onMessageReceived);
}