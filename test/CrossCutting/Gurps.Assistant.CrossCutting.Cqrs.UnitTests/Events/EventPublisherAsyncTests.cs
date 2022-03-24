using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gurps.Assistant.CrossCutting.Cqrs.Bus;
using Gurps.Assistant.CrossCutting.Cqrs.Dependencies;
using Gurps.Assistant.CrossCutting.Cqrs.Events;
using Gurps.Assistant.CrossCutting.Cqrs.UnitTests.Fakes;
using Moq;
using Xunit;

namespace Gurps.Assistant.CrossCutting.Cqrs.UnitTests.Events
{
  public class EventPublisherAsyncTests
  {

    private readonly IEventPublisher sut;

    private readonly Mock<IResolver> resolver;
    private readonly Mock<IBusMessageDispatcher> busMessageDispatcher;

    private readonly Mock<IEventHandlerAsync<SomethingCreated>> eventHandler1;
    private readonly Mock<IEventHandlerAsync<SomethingCreated>> eventHandler2;

    private SomethingCreated somethingCreated;


    public EventPublisherAsyncTests()
    {
      somethingCreated = new SomethingCreated();

      eventHandler1 = new Mock<IEventHandlerAsync<SomethingCreated>>();
      eventHandler1
        .Setup(x => x.HandleAsync(somethingCreated))
        .Returns(Task.CompletedTask);

      eventHandler2 = new Mock<IEventHandlerAsync<SomethingCreated>>();
      eventHandler2
        .Setup(x => x.HandleAsync(somethingCreated))
        .Returns(Task.CompletedTask);

      resolver = new Mock<IResolver>();
      resolver
        .Setup(x => x.ResolveAll<IEventHandlerAsync<SomethingCreated>>())
        .Returns(new List<IEventHandlerAsync<SomethingCreated>> { eventHandler1.Object, eventHandler2.Object });

      busMessageDispatcher = new Mock<IBusMessageDispatcher>();
      busMessageDispatcher
        .Setup(x => x.DispatchAsync(somethingCreated))
        .Returns(Task.CompletedTask);

      sut = new EventPublisher(resolver.Object, busMessageDispatcher.Object);
    }

    [Fact]
    public void PublishAsync_ThrowsException_WhenEventIsNull()
    {
      somethingCreated = null;
      Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.PublishAsync(somethingCreated));
    }

    [Fact]
    public async Task PublishAsync_PublishesFirstEvent()
    {
      await sut.PublishAsync(somethingCreated);
      eventHandler1.Verify(x => x.HandleAsync(somethingCreated), Times.Once);
    }

    [Fact]
    public async Task PublishAsync_PublishesSecondEvent()
    {
      await sut.PublishAsync(somethingCreated);
      eventHandler2.Verify(x => x.HandleAsync(somethingCreated), Times.Once);
    }

    [Fact]
    public async Task PublishAsync_DispatchesEventToServiceBus()
    {
      await sut.PublishAsync(somethingCreated);
      busMessageDispatcher.Verify(x => x.DispatchAsync(It.IsAny<IBusMessage>()), Times.Once);
    }
  }
}
