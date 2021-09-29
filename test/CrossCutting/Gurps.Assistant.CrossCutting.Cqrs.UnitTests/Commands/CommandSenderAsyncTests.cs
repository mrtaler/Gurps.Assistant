using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Gurps.Assistant.CrossCutting.Cqrs.Commands;
using Gurps.Assistant.CrossCutting.Cqrs.Dependencies;
using Gurps.Assistant.CrossCutting.Cqrs.Domain;
using Gurps.Assistant.CrossCutting.Cqrs.Events;
using Gurps.Assistant.CrossCutting.Cqrs.UnitTests.Fakes;
using Gurps.Assistant.CrossCutting.Cqrs.Validation;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Gurps.Assistant.CrossCutting.Cqrs.UnitTests.Commands
{
  public class CommandSenderAsyncTests
  {
    private readonly ICommandSender sut;

    private readonly Mock<IHandlerResolver> handlerResolver;
    private readonly Mock<IEventPublisher> eventPublisher;
    private readonly Mock<IStoreProvider> storeProvider;
    private readonly Mock<IEventFactory> objectFactory;
    private readonly Mock<IValidationService> validationService;

    private readonly Mock<ICommandHandlerAsync<CreateSomething>> commandHandlerAsync;
    private readonly Mock<ICommandHandlerAsync<CreateAggregate>> domainCommandHandlerAsync;
    private readonly Mock<ISequenceCommandHandlerAsync<CommandInSequence>> sequenceCommandHandlerAsync;
    private readonly Mock<IOptions<Configuration.Options>> mainOptionsMock;

    private readonly CreateSomething createSomething;
    private readonly CreateSomething createSomethingConcrete;
    private readonly SomethingCreated somethingCreated;
    private readonly SomethingCreated somethingCreatedConcrete;
    private readonly IEnumerable<IEvent> events;

    private CreateAggregate createAggregate;
    private readonly CreateAggregate createAggregateConcrete;
    private readonly AggregateCreated aggregateCreated;
    private readonly AggregateCreated aggregateCreatedConcrete;
    private readonly Aggregate aggregate;

    private readonly SampleCommandSequence sampleCommandSequence;
    private readonly CommandInSequence commandInSequenceConcrete;

    private readonly CommandResponse commandResponse;
    private readonly CommandResponse domainCommandResponse;

    private SaveStoreData storeDataSaved;


    public CommandSenderAsyncTests()
    {
      createSomething = new CreateSomething();
      createSomethingConcrete = new CreateSomething();

      somethingCreated = new SomethingCreated();
      somethingCreatedConcrete = new SomethingCreated();

      events = new List<IEvent> { somethingCreated };

      createAggregate = new CreateAggregate();
      createAggregateConcrete = new CreateAggregate();

      aggregateCreatedConcrete = new AggregateCreated();
      aggregate = new Aggregate();
      aggregateCreated = (AggregateCreated)aggregate.Events[0];

      sampleCommandSequence = new SampleCommandSequence();
      commandInSequenceConcrete = new CommandInSequence();

      commandResponse = new CommandResponse { Events = events, Result = "Result" };
      domainCommandResponse = new CommandResponse { Events = aggregate.Events, Result = "Result" };

      eventPublisher = new Mock<IEventPublisher>();
      eventPublisher
          .Setup(x => x.PublishAsync(aggregateCreatedConcrete))
          .Returns(Task.CompletedTask);

      storeProvider = new Mock<IStoreProvider>();
      storeProvider
          .Setup(x => x.SaveAsync(It.IsAny<SaveStoreData>()))
          .Callback<SaveStoreData>(x => storeDataSaved = x)
          .Returns(Task.CompletedTask);

      objectFactory = new Mock<IEventFactory>();
      objectFactory
          .Setup(x => x.CreateConcreteEvent(somethingCreated))
          .Returns(somethingCreatedConcrete);
      objectFactory
          .Setup(x => x.CreateConcreteEvent(aggregateCreated))
          .Returns(aggregateCreatedConcrete);
      objectFactory
          .Setup(x => x.CreateConcreteEvent(createSomething))
          .Returns(createSomethingConcrete);
      objectFactory
          .Setup(x => x.CreateConcreteEvent(createAggregate))
          .Returns(createAggregateConcrete);
      objectFactory
          .Setup(x => x.CreateConcreteEvent(It.IsAny<CommandInSequence>()))
          .Returns(commandInSequenceConcrete);

      validationService = new Mock<IValidationService>();
      validationService
          .Setup(x => x.ValidateAsync(It.IsAny<CreateSomething>()))
          .Returns(Task.CompletedTask);

      commandHandlerAsync = new Mock<ICommandHandlerAsync<CreateSomething>>();

      commandHandlerAsync
          .Setup(x => x.HandleAsync(createSomethingConcrete))
          .ReturnsAsync(commandResponse);

      domainCommandHandlerAsync = new Mock<ICommandHandlerAsync<CreateAggregate>>();
      domainCommandHandlerAsync
          .Setup(x => x.HandleAsync(createAggregate))
          .ReturnsAsync(domainCommandResponse);
      domainCommandHandlerAsync
          .Setup(x => x.HandleAsync(createAggregateConcrete))
          .ReturnsAsync(domainCommandResponse);

      sequenceCommandHandlerAsync = new Mock<ISequenceCommandHandlerAsync<CommandInSequence>>();
      sequenceCommandHandlerAsync
          .Setup(x => x.HandleAsync(It.IsAny<CommandInSequence>(), It.IsAny<CommandResponse>()))
          .ReturnsAsync(commandResponse);

      handlerResolver = new Mock<IHandlerResolver>();

      handlerResolver
          .Setup(x => x.ResolveHandler(createSomething, typeof(ICommandHandlerAsync<>)))
          .Returns(commandHandlerAsync.Object);
      handlerResolver
          .Setup(x => x.ResolveHandler<ICommandHandlerAsync<CreateSomething>>())
          .Returns(commandHandlerAsync.Object);

      handlerResolver
          .Setup(x => x.ResolveHandler<ICommandHandlerAsync<CreateAggregate>>())
          .Returns(domainCommandHandlerAsync.Object);
      handlerResolver
          .Setup(x => x.ResolveHandler<ISequenceCommandHandlerAsync<CommandInSequence>>())
          .Returns(sequenceCommandHandlerAsync.Object);

      mainOptionsMock = new Mock<IOptions<Configuration.Options>>();
      mainOptionsMock
          .Setup(x => x.Value)
          .Returns(new Configuration.Options());

      sut = new CommandSender(
        handlerResolver.Object,
          eventPublisher.Object,
          objectFactory.Object,
          storeProvider.Object,
          validationService.Object,
          mainOptionsMock.Object);
    }

    [Fact]
    public void SendAsync_ThrowsException_WhenCommandIsNull()
    {
      createAggregate = null;
      Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.SendAsync(createAggregate));
    }

    [Fact]
    public async Task CreateSomething_SendAsync_ValidatesCommand()
    {
      createSomething.Validate = true;
      await sut.SendAsync(createSomething);
      validationService.Verify(x => x.ValidateAsync(It.IsAny<CreateSomething>()), Times.Once);
    }

    [Fact]
    public async Task CreateSomething_SendAsync_HandlesCommand()
    {
      await sut.SendAsync(createSomething);
      commandHandlerAsync.Verify(x => x.HandleAsync(createSomething), Times.Once);
    }

   /* [Fact]
    public async Task CreateAggregate_SendAsync_HandlesDomainCommand()
    {
      await sut.SendAsync(createAggregate);
      domainCommandHandlerAsync.Verify(x => x.HandleAsync(createAggregate), Times.Once);
    }

    [Fact]
    public async Task SampleCommandSequence_SendAsync_HandlesCommand_InCommandSequence()
    {
      await sut.SendAsync(sampleCommandSequence);
      sequenceCommandHandlerAsync.Verify(x => x.HandleAsync(It.IsAny<CommandInSequence>(), It.IsAny<CommandResponse>()), Times.Once);
    }

    [Fact]
    public async Task CreateAggregate_SendAsync_SavesStoreData()
    {
      await sut.SendAsync(createAggregate);
      storeProvider.Verify(x => x.SaveAsync(It.IsAny<SaveStoreData>()), Times.Once);
    }

    [Fact]
    public async Task CreateAggregate_SendAsync_SavesCorrectData()
    {
      await sut.SendAsync(createAggregate);
      storeDataSaved.AggregateType.Should().BeOfType(aggregate.GetType());
      storeDataSaved.AggregateRootId.Should().Be(createAggregate.AggregateRootId);
      storeDataSaved.Events.FirstOrDefault().Should().Be(aggregateCreated);
      storeDataSaved.DomainCommand.Should().Be(createAggregate);

      //Assert.AreEqual(_aggregate.GetType(), _storeDataSaved.AggregateType);
      //Assert.AreEqual(_createAggregate.AggregateRootId, _storeDataSaved.AggregateRootId);
      //Assert.AreEqual(_aggregateCreated, _storeDataSaved.Events.FirstOrDefault());
      //Assert.AreEqual(_createAggregate, _storeDataSaved.DomainCommand);

    }

    [Fact]
    public async Task CreateAggregate_SendAsync_PublishesEvents()
    {
      await sut.SendAsync(createAggregate);
      eventPublisher.Verify(x => x.PublishAsync(aggregateCreatedConcrete), Times.Once);
    }

    [Fact]
    public async Task CreateAggregate_SendAsync_NotPublishesEvents_WhenSetInOptions()
    {
      mainOptionsMock
          .Setup(x => x.Value)
          .Returns(new Configuration.Options { PublishEvents = false });

      sut = new CommandSender(handlerResolver.Object,
          eventPublisher.Object,
          objectFactory.Object,
          storeProvider.Object,
          new Mock<IValidationService>().Object,
          mainOptionsMock.Object);

      await sut.SendAsync(createAggregate);
      eventPublisher.Verify(x => x.PublishAsync(aggregateCreatedConcrete), Times.Never);
    }

    [Fact]
    public async Task CreateAggregate_SendAsync_NotPublishesEvents_WhenSetInCommand()
    {
      createAggregate.PublishEvents = false;
      await sut.SendAsync(createAggregate);
      eventPublisher.Verify(x => x.PublishAsync(aggregateCreatedConcrete), Times.Never);
    }

    [Fact]
    public async Task CreateSomething_SendAsyncWithResult_ReturnsResult()
    {
      var actual = await sut.SendAsync<string>(createSomething);
      actual.Should().Be("Result");

      // Assert.AreEqual("Result", actual);
    }*/
  }
}
