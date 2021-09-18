﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects.Assert;
using Xunit;

namespace Gurps.Assistant.Domain.Repository.UnitTests.Caching
{
  public class HashGeneratorTests : TestBase
  {
    [Fact]
    public void Same_Predicate_Will_Give_Same_Hash()
    {
      Expression<Func<Contact, bool>> predicate = contact => contact.Name == "test" && contact.ContactId == 1;
      Expression<Func<Contact, bool>> predicate2 = contact => contact.Name == "test" && contact.ContactId == 1;

      var hash1 = HashGenerator.FromSpecification(new Specification<Contact>(predicate));
      var hash2 = HashGenerator.FromSpecification(new Specification<Contact>(predicate2));

      hash1.Should().Be(hash2);
    }

    [Fact]
    public void Same_Specification_Will_Give_Same_Hash()
    {
      var spec = new Specification<Contact>(p => p.ContactId == 1)
          .And(new Specification<Contact>(p => p.Name.Equals("test")));

      var spec2 = new Specification<Contact>(p => p.ContactId == 1)
          .And(new Specification<Contact>(p => p.Name.Equals("test")));

      var hash1 = HashGenerator.FromSpecification(spec);
      var hash2 = HashGenerator.FromSpecification(spec2);

      hash1.Should().Be(hash2);
    }

    [Fact]
    public void Different_Predicate_Will_Give_Different_Hash()
    {
      Expression<Func<Contact, bool>> predicate = contact => contact.Name == "test" && contact.ContactId == 1;
      Expression<Func<Contact, bool>> predicate2 = contact => contact.Name == "test" && contact.ContactId == 2;

      var hash1 = HashGenerator.FromSpecification(new Specification<Contact>(predicate));
      var hash2 = HashGenerator.FromSpecification(new Specification<Contact>(predicate2));

      hash1.Should().NotBe(hash2);
    }

    [Fact]
    public void Different_Specification_Param_Will_Give_Different_Hash()
    {
      var spec = new Specification<Contact>(p => p.ContactId == 1)
          .And(new Specification<Contact>(p => p.Name.Equals("test")));

      var spec2 = new Specification<Contact>(p => p.ContactId == 2)
          .And(new Specification<Contact>(p => p.Name.Equals("test")));

      var hash1 = HashGenerator.FromSpecification(spec);
      var hash2 = HashGenerator.FromSpecification(spec2);

      hash1.Should().NotBe(hash2);
    }

    [Fact]
    public void Same_Predicate_With_Variables_Will_Give_Same_Hash()
    {
      var name = "test";
      var contactId = 1;

      Expression<Func<Contact, bool>> predicate = contact => contact.Name == name && contact.ContactId == contactId;
      var hash1 = HashGenerator.FromSpecification(new Specification<Contact>(predicate));

      Expression<Func<Contact, bool>> predicate2 = contact => contact.Name == name && contact.ContactId == contactId;
      var hash2 = HashGenerator.FromSpecification(new Specification<Contact>(predicate2));

      hash1.Should().Be(hash2);
    }

    [Fact]
    public void Different_Predicate_With_Variables_Will_Give_Different_Hash()
    {
      var name = "test";
      var contactId = 1;

      Expression<Func<Contact, bool>> predicate = contact => contact.Name == name && contact.ContactId == contactId;
      var hash1 = HashGenerator.FromSpecification(new Specification<Contact>(predicate));

      contactId = 2;
      Expression<Func<Contact, bool>> predicate2 = contact => contact.Name == name && contact.ContactId == contactId;
      var hash2 = HashGenerator.FromSpecification(new Specification<Contact>(predicate2));

      hash1.Should().NotBe(hash2);
    }

    [Fact]
    public void Same_Predicate_With_Variable_Array_Will_Give_Same_Hash()
    {
      var name = "test";
      var contactIds = new List<int> { 1, 2 };

      Expression<Func<Contact, bool>> predicate = contact => contact.Name == name && contactIds.Contains(contact.ContactId);
      var hash1 = HashGenerator.FromSpecification(new Specification<Contact>(predicate));

      var contactIds2 = new List<int> { 1, 2 };
      Expression<Func<Contact, bool>> predicate2 = contact => contact.Name == name && contactIds2.Contains(contact.ContactId);
      var hash2 = HashGenerator.FromSpecification(new Specification<Contact>(predicate2));

      hash1.Should().Be(hash2);
    }

    [Fact]
    public void Different_Predicate_With_Variable_Array_Will_Give_Different_Hash()
    {
      var name = "test";
      var contactIds = new List<int> { 1, 2 };

      Expression<Func<Contact, bool>> predicate = contact => contact.Name == name && contactIds.Contains(contact.ContactId);
      var hash1 = HashGenerator.FromSpecification(new Specification<Contact>(predicate));

      contactIds = new List<int> { 3, 4 };
      Expression<Func<Contact, bool>> predicate2 = contact => contact.Name == name && contactIds.Contains(contact.ContactId);
      var hash2 = HashGenerator.FromSpecification(new Specification<Contact>(predicate2));

      hash1.Should().Be(hash2);
    }

    [Fact]
    public void Different_Specification_Ordering_Will_Give_Different_Hash()
    {
      var spec = new Specification<Contact>(p => p.ContactId == 1)
          .And(new Specification<Contact>(p => p.Name.Equals("test")));

      var spec2 = new Specification<Contact>(p => p.Name.Equals("test"))
          .And(new Specification<Contact>(p => p.ContactId == 1));

      var hash1 = HashGenerator.FromSpecification(spec);
      var hash2 = HashGenerator.FromSpecification(spec2);

      hash1.Should().NotBe(hash2);
    }
  }
}
