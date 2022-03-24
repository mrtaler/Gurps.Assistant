using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gurps.Assistant.CrossCutting.Cqrs.Domain;
using Gurps.Assistant.CrossCutting.Cqrs.Store.EF.Entities.Factories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Gurps.Assistant.CrossCutting.Cqrs.Store.EF
{
  public class StoreProvider : IStoreProvider
  {
    private readonly IDomainDbContextFactory dbContextFactory;
    private readonly IAggregateEntityFactory aggregateEntityFactory;
    private readonly ICommandEntityFactory commandEntityFactory;
    private readonly IEventEntityFactory eventEntityFactory;
    private readonly IVersionService versionService;

    public StoreProvider(IDomainDbContextFactory dbContextFactory,
        IAggregateEntityFactory aggregateEntityFactory,
        ICommandEntityFactory commandEntityFactory,
        IEventEntityFactory eventEntityFactory,
        IVersionService versionService)
    {
      this.dbContextFactory = dbContextFactory;
      this.aggregateEntityFactory = aggregateEntityFactory;
      this.commandEntityFactory = commandEntityFactory;
      this.eventEntityFactory = eventEntityFactory;
      this.versionService = versionService;
    }

    public IEnumerable<IDomainEvent> GetEvents(Guid aggregateId)
    {
      var result = new List<DomainEvent>();

      using (var dbContext = dbContextFactory.CreateDbContext())
      {
        var events = dbContext.Events
                              .Where(x => x.AggregateId == aggregateId)
                              .OrderBy(x => x.Sequence)
                              .ToList();

        foreach (var @event in events)
        {
          var domainEvent = JsonConvert.DeserializeObject(@event.Data, Type.GetType(@event.Type));
          result.Add((DomainEvent)domainEvent);
        }
      }

      return result;
    }

    public async Task<IEnumerable<IDomainEvent>> GetEventsAsync(Guid aggregateId)
    {
      var result = new List<DomainEvent>();

      using (var dbContext = dbContextFactory.CreateDbContext())
      {
        var events = await dbContext.Events
                                    .Where(x => x.AggregateId == aggregateId)
                                    .OrderBy(x => x.Sequence)
                                    .ToListAsync();

        foreach (var @event in events)
        {
          var domainEvent = JsonConvert.DeserializeObject(@event.Data, Type.GetType(@event.Type));
          result.Add((DomainEvent)domainEvent);
        }
      }

      return result;
    }

    public void Save(SaveStoreData request)
    {
      using var dbContext = dbContextFactory.CreateDbContext();
      var aggregateEntity = dbContext.Aggregates.FirstOrDefault(x => x.Id == request.AggregateRootId.ToString());
      if (aggregateEntity == null)
      {
        var newAggregateEntity = aggregateEntityFactory.CreateAggregate(request.AggregateType, request.AggregateRootId);
        dbContext.Aggregates.Add(newAggregateEntity);
      }

      if (request.DomainCommand != null)
      {
        var newCommandEntity = commandEntityFactory.CreateCommand(request.DomainCommand);
        dbContext.Commands.Add(newCommandEntity);
      }

      foreach (var @event in request.Events)
      {
        var currentVersion = dbContext.Events.Count(x => x.AggregateId == @event.AggregateRootId);
        var nextVersion = versionService.GetNextVersion(@event.AggregateRootId, currentVersion, request.DomainCommand?.ExpectedVersion);
        var newEventEntity = eventEntityFactory.CreateEvent(@event, nextVersion);
        dbContext.Events.Add(newEventEntity);
      }

      dbContext.SaveChanges();
    }

    public async Task SaveAsync(SaveStoreData request)
    {
      using var dbContext = dbContextFactory.CreateDbContext();
      var aggregateEntity = await dbContext.Aggregates.FirstOrDefaultAsync(x => x.Id == request.AggregateRootId.ToString());
      if (aggregateEntity == null)
      {
        var newAggregateEntity = aggregateEntityFactory.CreateAggregate(request.AggregateType, request.AggregateRootId);
        await dbContext.Aggregates.AddAsync(newAggregateEntity);
      }

      if (request.DomainCommand != null)
      {
        var newCommandEntity = commandEntityFactory.CreateCommand(request.DomainCommand);
        await dbContext.Commands.AddAsync(newCommandEntity);
      }

      foreach (var @event in request.Events)
      {
        var currentVersion = await dbContext.Events.CountAsync(x => x.AggregateId == @event.AggregateRootId);
        var nextVersion = versionService.GetNextVersion(@event.AggregateRootId, currentVersion, request.DomainCommand?.ExpectedVersion);
        var newEventEntity = eventEntityFactory.CreateEvent(@event, nextVersion);
        await dbContext.Events.AddAsync(newEventEntity);
      }

      await dbContext.SaveChangesAsync();
    }
  }
}