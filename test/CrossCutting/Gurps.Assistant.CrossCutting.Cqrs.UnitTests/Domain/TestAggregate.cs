using Gurps.Assistant.CrossCutting.Cqrs.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Gurps.Assistant.CrossCutting.Cqrs.UnitTests.Domain
{
  public class TestAggregate : AggregateRoot
  {
    public int Number { get; private set; }
    private readonly List<Item> _lineItems = new();
    public ReadOnlyCollection<Item> Items => _lineItems.AsReadOnly();

    public TestAggregate()
    {
    }

    public TestAggregate(int number)
    {
      AddAndApplyEvent(new TestAggregateCreated(number));
    }

    private void Apply(TestAggregateCreated @event)
    {
      Number = @event.Number;
    }

    public void AddItem(string v1, int v2, double v3, bool v4)
    {
      AddAndApplyEvent(new ItemAdded()
      {
        AggregateRootId = Id,
        Description = v1,
        Quantity = v2,
        Price = v3,
        Taxable = v4
      });
    }

    private void Apply(ItemAdded @event)
    {
      _lineItems.Add(new Item(Version, @event.Description, @event.Quantity, @event.Price, @event.Taxable));
    }

    public void RemoveItem(int id)
    {
      AddAndApplyEvent(new ItemRemoved()
      {
        AggregateRootId = Id,
        ItemId = id
      });
    }

    private void Apply(ItemRemoved @event)
    {
      var item = _lineItems.SingleOrDefault(l => l.Id == @event.ItemId);
      if (item != null)
      {
        _lineItems.Remove(item);
      }
    }

    public void ChangeItem(int id, string description)
    {
      AddAndApplyEvent(new ItemDescriptionChanged()
      {
        AggregateRootId = Id,
        ItemId = id,
        Description = description
      });
    }

    private void Apply(ItemDescriptionChanged @event)
    {
      var item = _lineItems.SingleOrDefault(l => l.Id == @event.ItemId);
      if (item != null)
      {
        _lineItems.Remove(item);
      }
      _lineItems.Add(new Item(item.Id, @event.Description, item.Quantity, item.Price, item.Taxable));
    }

    public void ChangeItem(int id, int quantity)
    {
      AddAndApplyEvent(new ItemQuantityChanged()
      {
        AggregateRootId = Id,
        ItemId = id,
        Quantity = quantity
      });
    }

    private void Apply(ItemQuantityChanged @event)
    {
      var item = _lineItems.SingleOrDefault(l => l.Id == @event.ItemId);
      if (item != null)
      {
        _lineItems.Remove(item);
      }
      _lineItems.Add(new Item(item.Id, item.Description, @event.Quantity, item.Price, item.Taxable));
    }

  }
}