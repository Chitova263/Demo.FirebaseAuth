using Demo.Common.Events;

namespace Demo.Common.Rabbitmq;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event, string exchangeName, string routingKey) where TEvent : IEvent;
}
