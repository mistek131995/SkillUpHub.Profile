namespace SkillUpHub.Command.Infrastructure.Interfaces;

public interface IMessageBusClient
{
    /// <summary>
    /// Метод инициализации очередей и обменников
    /// </summary>
    void Initialize();
    void PublishMessage<T>(T message, string exchange, string routingKey);
    void PublishErrorMessage(Exception exception);
    void Subscribe<T>(string queueName, Func<T, Task> onMessageReceived);
}