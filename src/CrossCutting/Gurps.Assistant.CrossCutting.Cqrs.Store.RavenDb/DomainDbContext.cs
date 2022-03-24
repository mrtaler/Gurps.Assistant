using Gurps.Assistant.CrossCutting.Cqrs.Store.RavenDb.Documents;
using Gurps.Assistant.Domain.Repository.Interfaces.Repository;

namespace Gurps.Assistant.CrossCutting.Cqrs.Store.RavenDb
{
  public class DomainDbContext
  {
    /// <summary>
    /// The aggregates.
    /// </summary>
    private readonly IRepository<AggregateDocument, string> aggregates;

    /// <summary>
    /// The commands.
    /// </summary>
    private readonly IRepository<CommandDocument, string> commands;

    /// <summary>
    /// The events.
    /// </summary>
    private readonly IRepository<EventDocument, string> events;

    public DomainDbContext(
        IRepository<AggregateDocument, string> aggregates,
        IRepository<CommandDocument, string> commands,
        IRepository<EventDocument, string> events)
    {
      this.aggregates = aggregates;
      this.commands = commands;
      this.events = events;
    }

    public IRepository<AggregateDocument, string> Aggregates => aggregates;

    public IRepository<CommandDocument, string> Commands => commands;

    public IRepository<EventDocument, string> Events => events;
  }
}