﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Gurps.Assistant.Domain.Repository.Queries;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects.Assert;
using Xunit;

namespace Gurps.Assistant.Domain.Repository.UnitTests.QueryOptions
{

  public class SortingOptionsTests : TestBase
  {
    [Fact]
    public void SortingOptions_Will_Sort_By_SortProperty_Asc()
    {
      var contacts = new List<Contact>();
      for (var i = 5; i >= 1; i--)
      {
        contacts.Add(new Contact { Name = "Test User " + i });
      }

      var qo = new SortingOptions<Contact>("Name");
      var queryable = qo.Apply(contacts.AsQueryable());
      queryable.Count().Should().Be(5);
      queryable.First().Name.Should().Be("Test User 1");
    }

    [Fact]
    public void SortingOptions_Will_Sort_By_SortProperty_Desc()
    {
      var contacts = new List<Contact>();
      for (var i = 1; i <= 5; i++)
      {
        contacts.Add(new Contact { Name = "Test User " + i });
      }

      var qo = new SortingOptions<Contact>("Name", true);
      var queryable = qo.Apply(contacts.AsQueryable());
      queryable.Count().Should().Be(5);
      queryable.First().Name.Should().Be("Test User 5");
    }

    [Fact]
    public void SortingOptions_With_Multiple_Sorting_Properties()
    {
      var contacts = new List<Contact>();
      for (var i = 5; i >= 1; i--)
      {
        contacts.Add(new Contact { Name = "Test User " + i % 2, ContactTypeId = i });
      }

      var qo = new SortingOptions<Contact>("Name");
      qo.ThenSortBy("ContactTypeId");

      var queryable = qo.Apply(contacts.AsQueryable());
      queryable.Count().Should().Be(5);

      var contact = queryable.First();
      contact.Name.Should().Be("Test User 0");
      contact.ContactTypeId.Should().Be(2);
    }

    [Fact]
    public void SortingOptions_Will_Sort_By_SortExpression_Asc()
    {
      var contacts = new List<Contact>();
      for (var i = 5; i >= 1; i--)
      {
        contacts.Add(new Contact { Name = "Test User " + i });
      }

      var qo = new SortingOptions<Contact, string>(x => x.Name);
      var queryable = qo.Apply(contacts.AsQueryable());
      queryable.Count().Should().Be(5);
      queryable.First().Name.Should().Be("Test User 1");
    }

    [Fact]
    public void SortingOptions_Will_Sort_By_SortExpression_Desc()
    {
      var contacts = new List<Contact>();
      for (var i = 1; i <= 5; i++)
      {
        contacts.Add(new Contact { Name = "Test User " + i });
      }

      var qo = new SortingOptions<Contact, string>(x => x.Name, isDescending: true);
      var queryable = qo.Apply(contacts.AsQueryable());
      queryable.Count().Should().Be(5);
      queryable.First().Name.Should().Be("Test User 5");
    }

    [Fact]
    public void SortingOptions_With_Multiple_SortExpression_Properties()
    {
      var contacts = new List<Contact>();
      for (var i = 5; i >= 1; i--)
      {
        contacts.Add(new Contact { Name = "Test User " + i % 2, ContactTypeId = i });
      }

      var qo = new SortingOptions<Contact, string>(x => x.Name);
      qo.ThenSortBy(x => x.ContactTypeId);

      var queryable = qo.Apply(contacts.AsQueryable());
      queryable.Count().Should().Be(5);

      var contact = queryable.First();
      contact.Name.Should().Be("Test User 0");
      contact.ContactTypeId.Should().Be(2);
    }

    [Fact]
    public void SortingOptions_Will_Sort_By_SortExpression_Multiple_Deep_Asc()
    {
      var contacts = new List<Contact>();
      for (var i = 1; i <= 5; i++)
      {
        contacts.Add(new Contact { Name = "Test User " + i, ContactType = new ContactType { Name = "Type " + (5 - i) } });
      }

      var qo = new SortingOptions<Contact, string>(x => x.ContactType.Name);
      var queryable = qo.Apply(contacts.AsQueryable());
      queryable.Count().Should().Be(5);
      queryable.First().Name.Should().Be("Test User 5");
    }

    [Fact]
    public void SortingOptions_Will_Sort_By_SortExpression_Multiple_Deep_Desc()
    {
      var contacts = new List<Contact>();
      for (var i = 1; i <= 5; i++)
      {
        contacts.Add(new Contact { Name = "Test User " + i, ContactType = new ContactType { Name = "Type " + (5 - i) } });
      }

      var qo = new SortingOptions<Contact, string>(x => x.ContactType.Name, isDescending: true);
      var queryable = qo.Apply(contacts.AsQueryable());
      queryable.Count().Should().Be(5);
      queryable.First().Name.Should().Be("Test User 1");
    }

    [Fact]
    public void SortingOptions_Will_Sort_By_Multiple_Deep_SortProperty_Asc()
    {
      var contacts = new List<Contact>();
      for (var i = 5; i >= 1; i--)
      {
        contacts.Add(new Contact { Name = "Test User " + i, ContactType = new ContactType { Name = "Type " + (5 - i) } });
      }

      var qo = new SortingOptions<Contact>("ContactType.Name");
      var queryable = qo.Apply(contacts.AsQueryable());
      queryable.Count().Should().Be(5);
      queryable.First().Name.Should().Be("Test User 5");
    }

    [Fact]
    public void SortingOptions_Will_Sort_By_Multiple_Deep_SortProperty_Desc()
    {
      var contacts = new List<Contact>();
      for (var i = 1; i <= 5; i++)
      {
        contacts.Add(new Contact { Name = "Test User " + i, ContactType = new ContactType { Name = "Type " + (5 - i) } });
      }

      var qo = new SortingOptions<Contact>("ContactType.Name", isDescending: true);
      var queryable = qo.Apply(contacts.AsQueryable());
      queryable.Count().Should().Be(5);
      queryable.First().Name.Should().Be("Test User 1");
    }
  }
}