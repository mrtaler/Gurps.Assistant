namespace Gurps.Assistant.CrossCutting.Cqrs.Bus
{
  public interface IBusTopicMessage : IBusMessage
  {
    string TopicName { get; set; }
  }
}
