using FluentAssertions;
using Xunit;

namespace Gurps.Assistant.CrossCutting.Cqrs.UnitTests.Domain
{
  public class AggregateRootTests
  {

    [Fact]
    public void CreateTestAggregate()
    {
      var sut = new TestAggregate(10);


      sut.Events.Count.Should().Be(1);
      sut.Version.Should().Be(1);
      sut.Number.Should().Be(10);

      /*
       Assert.AreEqual(1, sut.Events.Count, "Events");
       Assert.AreEqual(1, sut.Version, "Version");
       Assert.AreEqual(10, sut.Number, "Number");
      */
    }

    [Fact]
    public void AddItem()
    {
      var sut = new TestAggregate(100);

      sut.AddItem("Description", 1, 10.00, true);

      sut.Events.Count.Should().Be(2);
      sut.Version.Should().Be(2);
      sut.Items.Count.Should().Be(1);
      sut.Items[0].Description.Should().Be("Description");
      sut.Items[0].Quantity.Should().Be(1);
      sut.Items[0].Price.Should().Be(10.00);
      sut.Items[0].Taxable.Should().Be(true);

      /*
       Assert.AreEqual(2, sut.Events.Count, "Events");
       Assert.AreEqual(2, sut.Version, "Version");
       Assert.AreEqual(1, sut.Items.Count, "Items");
       Assert.AreEqual("Description", sut.Items[0].Description, "Description");
       Assert.AreEqual(1, sut.Items[0].Quantity, "Quantity");
       Assert.AreEqual(10.00, sut.Items[0].Price, "Price");
       Assert.AreEqual(true, sut.Items[0].Taxable, "Taxable");
      */
    }

    [Fact]
    public void RemoveItem()
    {
      var sut = new TestAggregate(100);
      sut.AddItem("Description", 1, 10.00, true);

      sut.RemoveItem(1);

      sut.Events.Count.Should().Be(3);
      sut.Version.Should().Be(3);
      sut.Items.Count.Should().Be(0);

      /*
      Assert.AreEqual(3, sut.Events.Count, "Events");
      Assert.AreEqual(3, sut.Version, "Version");
      Assert.AreEqual(0, sut.Items.Count, "Items");
      */
    }

    [Fact]
    public void ChangeItem()
    {
      var sut = new TestAggregate(100);
      sut.AddItem("Description", 1, 10.00, true);

      sut.ChangeItem(1, "New Description");
      sut.ChangeItem(1, 10);

      sut.Events.Count.Should().Be(4);
      sut.Version.Should().Be(4);
      sut.Items.Count.Should().Be(1);
      sut.Items[0].Description.Should().Be("New Description");
      sut.Items[0].Quantity.Should().Be(10);

      /*
      Assert.AreEqual(4, sut.Events.Count, "Events");
      Assert.AreEqual(4, sut.Version, "Version");
      Assert.AreEqual(1, sut.Items.Count, "Items");
      Assert.AreEqual("New Description", sut.Items[0].Description, "Description");
      Assert.AreEqual(10, sut.Items[0].Quantity, "Quantity");
      */
    }
  }
}
