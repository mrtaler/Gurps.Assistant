using System;

namespace Gurps.Assistant.CrossCutting.Cqrs.UnitTests.Domain
{
  public class Item
  {
    public Item(int id, string description, int quantity, double price, bool taxable)
    {
      Id = id;
      Description = description ?? throw new ArgumentNullException(nameof(description));
      Quantity = quantity;
      Price = price;
      Taxable = taxable;
    }

    public int Id { get; }
    public string Description { get; }
    public int Quantity { get; }
    public double Price { get; }
    public bool Taxable { get; }
  }
}