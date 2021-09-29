namespace Gurps.Assistant.CrossCutting.Cqrs.Bus
{
  public abstract class BusQueueMessage : BusMessage, IBusQueueMessage
  {
    public string QueueName { get; set; }
  }
}
