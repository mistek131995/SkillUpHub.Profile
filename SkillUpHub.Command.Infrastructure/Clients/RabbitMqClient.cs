using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SkillUpHub.Command.Infrastructure.Interfaces;

namespace SkillUpHub.Command.Infrastructure.Clients;

public class RabbitMqClient : IMessageBusClient
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqClient(string host)
    {
        var factory = new ConnectionFactory()
        {
            HostName = host
        };
        
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }
    
    public void PublishMessage<T>(T message, string routingKey)
    {
        _channel.QueueDeclare(queue: routingKey, durable: true, exclusive: false, autoDelete: false, arguments: null);
        
        var jsonMessage = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        _channel.BasicPublish(exchange: "",
            routingKey: routingKey,
            basicProperties: null,
            body: body);
    }
    
    public void Subscribe<T>(string queueName, Action<T> onMessageReceived)
    {
        _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var deserializedMessage = JsonConvert.DeserializeObject<T>(message);
            onMessageReceived(deserializedMessage);
        };
        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
    }
}