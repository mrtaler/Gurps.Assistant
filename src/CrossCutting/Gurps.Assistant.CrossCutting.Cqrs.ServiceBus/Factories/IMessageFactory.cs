using Gurps.Assistant.CrossCutting.Cqrs.Bus;
using Microsoft.Azure.ServiceBus;

namespace Gurps.Assistant.CrossCutting.Cqrs.ServiceBus.Factories
{
  /// <summary>
  /// IMessageFactory
  /// </summary>
  public interface IMessageFactory
  {
    /// <summary>
    /// Creates the service bus message.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message.</typeparam>
    /// <param name="message">The message.</param>
    /// <returns></returns>
    Message CreateMessage<TMessage>(TMessage message) where TMessage : IBusMessage;
  }
}
