namespace SkillUpHub.Command.Contract.Models;

public  class RabbitMqSettings
{
    public string Host { get; set; } 
    public List<Exchange> Exchanges { get; set; } 
    public List<Queue> Queues { get; set; }
    
    public record Exchange(string Id, string Name, string Type, bool Durable, bool AutoDelete, List<Queue> Queues);
    public record Queue(string Id, string Name, string Key, bool Exclusive, bool Durable, bool AutoDelete);
}