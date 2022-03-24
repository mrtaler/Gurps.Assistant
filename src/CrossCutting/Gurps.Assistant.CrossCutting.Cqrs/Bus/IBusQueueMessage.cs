namespace Gurps.Assistant.CrossCutting.Cqrs.Bus
{
  public interface IBusQueueMessage : IBusMessage
  {
    string QueueName { get; set; }
  }
}
