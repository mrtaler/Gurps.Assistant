using System;
using System.Threading.Tasks;
using Gurps.Assistant.CrossCutting.Cqrs.Bus;
using Gurps.Assistant.CrossCutting.Cqrs.ServiceBus.Factories;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace Gurps.Assistant.CrossCutting.Cqrs.ServiceBus
{
  public class BusProvider : IBusProvider
  {
    private readonly IMessageFactory messageFactory;
    private readonly string connectionString;

    public BusProvider(IMessageFactory messageFactory, IConfiguration configuration)
    {
      this.messageFactory = messageFactory;
      connectionString = configuration.GetConnectionString("KledexMessageBus");
    }

    /// <inheritdoc />
    public async Task SendQueueMessageAsync<TMessage>(TMessage message) where TMessage : IBusQueueMessage
    {
      if (string.IsNullOrEmpty(message.QueueName))
      {
        throw new ArgumentNullException("Queue name is mandatory");
      }

      var client = new QueueClient(new ServiceBusConnectionStringBuilder(connectionString)
      {
        EntityPath = message.QueueName
      });

      var serviceBusMessage = messageFactory.CreateMessage(message);

      await client.SendAsync(serviceBusMessage);

      await client.CloseAsync();
    }

    /// <inheritdoc />
    public async Task SendTopicMessageAsync<TMessage>(TMessage message) where TMessage : IBusTopicMessage
    {
      if (string.IsNullOrEmpty(message.TopicName))
        throw new ArgumentNullException("Topic name is mandatory");

      var client = new TopicClient(new ServiceBusConnectionStringBuilder(connectionString)
      {
        EntityPath = message.TopicName
      });

      var serviceBusMessage = messageFactory.CreateMessage(message);

      await client.SendAsync(serviceBusMessage);

      await client.CloseAsync();
    }
  }
}
