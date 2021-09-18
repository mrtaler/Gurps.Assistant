using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Gurps.Assistant.Domain.Repository.Queries;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects.Assert;
using Xunit;

namespace Gurps.Assistant.Domain.Repository.UnitTests.QueryOptions
{

  public class PagingOptionsTests : TestBase
  {
    [Fact]
    public void PagingOptions_PageNumber_Will_Be_Set_In_Constructor()
    {
      new PagingOptions<Contact>(1, 10, "Name").PageNumber.Should().Be(1);
      new PagingOptions<Contact, string>(1, 10, m => m.Name).PageNumber.Should().Be(1);
    }

    [Fact]
    public void PagingOptions_PageSize_Will_Be_Set_In_Constructor()
    {
      new PagingOptions<Contact>(1, 10, "Name").PageSize.Should().Be(10);
      new PagingOptions<Contact, string>(1, 10, m => m.Name).PageSize.Should().Be(10);
    }

    [Fact]
    public void PagingOptions_Apply_Will_Set_TotalItems()
    {
      var contacts = new List<Contact>();
      for (int i = 1; i <= 5; i++)
      {
        contacts.Add(new Contact { Name = "Test User " + i });
      }

      const int resultingPage = 2;
      const int pageSize = 2;
      var qo = new PagingOptions<Contact>(resultingPage, pageSize, "Name", isDescending: true);
      qo.Apply(contacts.AsQueryable());
      qo.TotalItems.Should().Be(5);

      var qo2 = new PagingOptions<Contact, string>(resultingPage, pageSize, x => x.Name, isDescending: true);
      qo2.Apply(contacts.AsQueryable());
      qo2.TotalItems.Should().Be(5);
    }

    [Fact]
    public void PagingOptions_Apply_Return_Requested_Page()
    {
      var contacts = new List<Contact>();
      for (int i = 1; i <= 5; i++)
      {
        contacts.Add(new Contact { Name = "Test User " + i });
      }

      const int resultingPage = 2;
      const int pageSize = 2;

      var qo = new PagingOptions<Contact>(resultingPage, pageSize, "Name", isDescending: true);
      IQueryable<Contact> queryable = qo.Apply(contacts.AsQueryable());
      queryable.Count().Should().Be(2);
      queryable.First().Name.Should().Be("Test User 3");

      var qo2 = new PagingOptions<Contact, string>(resultingPage, pageSize, x => x.Name, isDescending: true);
      queryable = qo2.Apply(contacts.AsQueryable());
      queryable.Count().Should().Be(2);
      queryable.First().Name.Should().Be("Test User 3");
    }

    [Fact]
    public void PagingOptions_Apply_Will_Set_TotalItems_With_Multiple_Sort()
    {
      var contacts = new List<Contact>();
      for (int i = 1; i <= 5; i++)
      {
        contacts.Add(new Contact { Name = "Test User " + i });
      }

      const int resultingPage = 2;
      const int pageSize = 2;
      var qo = new PagingOptions<Contact>(resultingPage, pageSize, "Name", isDescending: true);
      qo.ThenSortBy("ContactTypeId");
      qo.Apply(contacts.AsQueryable());
      qo.TotalItems.Should().Be(5);

      var qo2 = new PagingOptions<Contact, string>(resultingPage, pageSize, x => x.Name, isDescending: true);
      qo2.ThenSortBy(x => x.ContactTypeId);
      qo2.Apply(contacts.AsQueryable());
      qo2.TotalItems.Should().Be(5);
    }

    [Fact]
    public void PagingOptions_Apply_Return_Requested_Page_With_Multiple_Sort()
    {
      var contacts = new List<Contact>();
      for (int i = 1; i <= 5; i++)
      {
        contacts.Add(new Contact { Name = "Test User " + i % 2, ContactTypeId = i });
      }

      const int resultingPage = 2;
      const int pageSize = 2;

      var qo = new PagingOptions<Contact>(resultingPage, pageSize, "Name", isDescending: true);
      qo.ThenSortBy("ContactTypeId", isDescending: true);

      IQueryable<Contact> queryable = qo.Apply(contacts.AsQueryable());
      queryable.Count().Should().Be(2);

      var contact = queryable.First();
      contact.Name.Should().Be("Test User 1");
      contact.ContactTypeId.Should().Be(1);

      var qo2 = new PagingOptions<Contact, string>(resultingPage, pageSize, x => x.Name, isDescending: true);
      qo2.ThenSortBy(x => x.ContactTypeId, isDescending: true);

      queryable = qo2.Apply(contacts.AsQueryable());
      queryable.Count().Should().Be(2);

      contact = queryable.First();
      contact.Name.Should().Be("Test User 1");
      contact.ContactTypeId.Should().Be(1);
    }
  }
}