using System.Threading.Tasks;

namespace Gurps.Assistant.CrossCutting.Cqrs.Bus
{
  public interface IBusMessageDispatcher
  {
    Task DispatchAsync<TMessage>(TMessage message) where TMessage : IBusMessage;
  }
}
