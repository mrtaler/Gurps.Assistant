namespace Gurps.Assistant.CrossCutting.Cqrs.Store.RavenDb
{
  public class DomainDbOptions
  {
    public string DatabaseName { get; set; } = "DomainStore";
    public string AggregateCollectionName { get; set; } = "Aggregates";
    public string CommandCollectionName { get; set; } = "Commands";
    public string EventCollectionName { get; set; } = "Events";
  }
}