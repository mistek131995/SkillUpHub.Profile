using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SkillUpHub.Command.Contract.Models;
using SkillUpHub.Command.Infrastructure.Interfaces;

namespace SkillUpHub.Command.Infrastructure.Clients;

public class RabbitMqClient : IMessageBusClient
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IOptions<RabbitMqSettings> _options;
    
    public RabbitMqClient(IOptions<RabbitMqSettings> options)
    {
        var factory = new ConnectionFactory()
        {
            HostName = options.Value.Host
        };
        
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _options = options;
    }
    
    public void Initialize()
    {
        if (_options.Value.Exchanges.Count > 0)
        {
            foreach (var exchange in _options.Value.Exchanges)
            {
                _channel.ExchangeDeclare(exchange.Name, exchange.Type, exchange.Durable, exchange.AutoDelete);

                if (exchange.Queues.Count > 0)
                {
                    foreach (var queue in exchange.Queues)
                    {
                        _channel.QueueDeclare(queue.Name, queue.Durable, queue.AutoDelete, queue.Exclusive);
                        _channel.QueueBind(queue.Name, exchange.Name, queue.Key);
                    }
                }
            }
        }

        if (_options.Value.Queues.Count > 0)
        {
            foreach (var queue in _options.Value.Queues)
            {
                _channel.QueueDeclare(queue.Name, queue.Durable, queue.Exclusive, queue.AutoDelete);
            }
        }
    }

    public void PublishMessage<T>(T message, string exchange, string routingKey)
    {
        var jsonMessage = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        _channel.BasicPublish(exchange: exchange,
            routingKey: routingKey,
            basicProperties: null,
            body: body);
    }

    public void PublishErrorMessage(Exception exception)
    {
        _channel.QueueDeclare("logger", durable: true, exclusive: false, autoDelete: false, arguments: null);
        var jsonMessage = JsonConvert.SerializeObject(exception);
        var body = Encoding.UTF8.GetBytes(jsonMessage);

        _channel.BasicPublish(exchange: "",
            routingKey: "logger",
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