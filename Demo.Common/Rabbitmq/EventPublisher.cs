using Demo.Common.Events;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Demo.Common.Rabbitmq;

public class EventPublisher : IEventPublisher
{
    private readonly IModel _channel;
    public EventPublisher(ILogger<EventPublisher> _logger)
    {
        var connectionFactory = new ConnectionFactory
        {
            Uri = new Uri("amqps://wtjodomp:0OE0eikprg_-bStY5dDsl_a3O1MokOQQ@chimpanzee.rmq.cloudamqp.com/wtjodomp")
        };
        var connection = connectionFactory.CreateConnection();
        _channel = connection.CreateModel();
    }
    
    public Task PublishAsync<TEvent>(TEvent @event, string exchangeName, string routingKey) where TEvent : IEvent
    {
        throw new NotImplementedException();
    }
}