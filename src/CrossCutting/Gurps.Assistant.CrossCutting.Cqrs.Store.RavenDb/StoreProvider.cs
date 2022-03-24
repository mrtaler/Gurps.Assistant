using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gurps.Assistant.CrossCutting.Cqrs.Domain;
using Gurps.Assistant.CrossCutting.Cqrs.Store.RavenDb.Documents;
using Gurps.Assistant.CrossCutting.Cqrs.Store.RavenDb.Documents.Factories;
using Gurps.Assistant.Domain.Repository.Interfaces.Repository;
using Gurps.Assistant.Domain.Repository.Specifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Gurps.Assistant.CrossCutting.Cqrs.Store.RavenDb
{
  public class StoreProvider : IStoreProvider
  {
    private readonly DomainDbContext dbContext;
    private readonly IAggregateDocumentFactory aggregateDocumentFactory;
    private readonly ICommandDocumentFactory commandDocumentFactory;
    private readonly IEventDocumentFactory eventDocumentFactory;
    private readonly IVersionService versionService;

    public StoreProvider(IConfiguration configuration,
                         IOptions<DomainDbOptions> settings,
                         IAggregateDocumentFactory aggregateDocumentFactory,
                         ICommandDocumentFactory commandDocumentFactory,
                         IEventDocumentFactory eventDocumentFactory,
                         IVersionService versionService,

                          IRepository<AggregateDocument, string> aggregates,
                         IRepository<CommandDocument, string> commands,
                         IRepository<EventDocument, string> events)
    {
      dbContext = new DomainDbContext(aggregates, commands, events);
      this.aggregateDocumentFactory = aggregateDocumentFactory;
      this.commandDocumentFactory = commandDocumentFactory;
      this.commandDocumentFactory = commandDocumentFactory;
      this.eventDocumentFactory = eventDocumentFactory;
      this.versionService = versionService;
    }

    public IEnumerable<IDomainEvent> GetEvents(Guid aggregateId)
    {
      var result = new List<DomainEvent>();
      ISpecification<EventDocument> filter = new Specification<EventDocument>(x => x.AggregateId == aggregateId.ToString());

      //  var filter = Builders<EventDocument>.Filter.Eq("aggregateId", aggregateId.ToString());
      var events = dbContext.Events.FindAll(filter); //.ToList();

      foreach (var @event in events)
      {
        var domainEvent = JsonConvert.DeserializeObject(@event.Data, Type.GetType(@event.Type));
        result.Add((DomainEvent)domainEvent);
      }

      return result;
    }

    public async Task<IEnumerable<IDomainEvent>> GetEventsAsync(Guid aggregateId)
    {
      var result = new List<DomainEvent>();
      var filter = new Specification<EventDocument>(x => x.AggregateId == aggregateId.ToString());
      // var filter = Builders<EventDocument>.Filter.Eq("aggregateId", aggregateId.ToString());

      var events = await Task.Run(() => dbContext.Events.FindAll(filter));

      // var events = await this._dbContext.Events.Find(filter).ToListAsync();

      foreach (var @event in events)
      {
        var domainEvent = JsonConvert.DeserializeObject(@event.Data, Type.GetType(@event.Type));
        result.Add((DomainEvent)domainEvent);
      }

      return result;
    }

    public void Save(SaveStoreData request)
    {
      var aggregateFilter = new Specification<AggregateDocument>(x => x.Id == request.AggregateRootId.ToString());
      // var aggregateFilter = Builders<AggregateDocument>.Filter.Eq("_id", request.AggregateRootId.ToString());
      var aggregateDocument = dbContext.Aggregates.Find(aggregateFilter);
      if (aggregateDocument == null)
      {
        var newAggregateDocument = aggregateDocumentFactory.CreateAggregate(request.AggregateType, request.AggregateRootId);
        dbContext.Aggregates.Add(newAggregateDocument);
      }

      if (request.DomainCommand != null)
      {
        var commandDocument = commandDocumentFactory.CreateCommand(request.DomainCommand);
        dbContext.Commands.Add(commandDocument);
      }

      foreach (var @event in request.Events)
      {
        var eventFilter = new Specification<EventDocument>(x => x.AggregateId == @event.AggregateRootId.ToString());
        //  var eventFilter = Builders<EventDocument>.Filter.Eq("aggregateId", @event.AggregateRootId.ToString());
        var currentVersion = dbContext.Events.Count(eventFilter);
        var nextVersion = versionService.GetNextVersion(@event.AggregateRootId, currentVersion, request.DomainCommand?.ExpectedVersion);

        var eventDocument = eventDocumentFactory.CreateEvent(@event, nextVersion);

        dbContext.Events.Add(eventDocument);
      }
    }

    public async Task SaveAsync(SaveStoreData request)
    {
      var aggregateFilter = new Specification<AggregateDocument>(x => x.Id == request.AggregateRootId.ToString());
      // var aggregateFilter = Builders<AggregateDocument>.Filter.Eq("_id", request.AggregateRootId.ToString());
      var aggregateDocument = await Task.Run(() => dbContext.Aggregates.Find(aggregateFilter));
      if (aggregateDocument == null)
      {
        var newAggregateDocument = aggregateDocumentFactory.CreateAggregate(request.AggregateType, request.AggregateRootId);
        await Task.Run(() => dbContext.Aggregates.Add(newAggregateDocument));
      }

      if (request.DomainCommand != null)
      {
        var commandDocument = commandDocumentFactory.CreateCommand(request.DomainCommand);
        await Task.Run(() => dbContext.Commands.Add(commandDocument));
      }


      foreach (var @event in request.Events)
      {
        var eventFilter = new Specification<EventDocument>(x => x.AggregateId == @event.AggregateRootId.ToString());
        // var eventFilter = Builders<EventDocument>.Filter.Eq("aggregateId", @event.AggregateRootId.ToString());
        var currentVersion = dbContext.Events.Count(eventFilter);
        var nextVersion = versionService.GetNextVersion(@event.AggregateRootId, currentVersion, request.DomainCommand?.ExpectedVersion);

        var eventDocument = eventDocumentFactory.CreateEvent(@event, nextVersion);


      }

    }
  }
}