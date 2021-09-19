using FluentAssertions;
using Gurps.Assistant.Domain.Repository.FetchStrategies;
using Gurps.Assistant.Domain.Repository.Specifications;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects.Assert;
using Xunit;

namespace Gurps.Assistant.Domain.Repository.UnitTests.Specifications
{
  public class SpecificationTests : TestBase
  {
    [Fact]
    public void Specification_Will_Default_To_GenericFetchStrategy()
    {
      var spec = new Specification<Contact>(p => p.ContactId == 1);
      spec.FetchStrategy.Should().BeOfType<GenericFetchStrategy<Contact>>();
    }

    [Fact]
    public void Specification_May_Be_Chained_By_And()
    {
      var spec = new Specification<Contact>(p => p.ContactId == 1)
          .And(new Specification<Contact>(p => p.Name.Equals("test")));

      var contact = new Contact() { ContactId = 1, Name = "test" };
      spec.IsSatisfiedBy(contact).Should().BeTrue();

      contact = new Contact() { ContactId = 2, Name = "test" };
      spec.IsSatisfiedBy(contact).Should().BeFalse();

      contact = new Contact() { ContactId = 1, Name = "nottest" };
      spec.IsSatisfiedBy(contact).Should().BeFalse();
    }

    [Fact]
    public void Specification_May_Be_Chained_By_Or()
    {
      var spec = new Specification<Contact>(p => p.ContactId == 1)
          .Or(new Specification<Contact>(p => p.Name.Equals("test")));

      var contact = new Contact() { ContactId = 1, Name = "test" };
      spec.IsSatisfiedBy(contact).Should().BeTrue();

      contact = new Contact() { ContactId = 2, Name = "test" };
      spec.IsSatisfiedBy(contact).Should().BeTrue();

      contact = new Contact() { ContactId = 1, Name = "nottest" };
      spec.IsSatisfiedBy(contact).Should().BeTrue();

      contact = new Contact() { ContactId = 2, Name = "nottest" };
      spec.IsSatisfiedBy(contact).Should().BeFalse();
    }

    [Fact]
    public void Specification_Predicate_May_Be_Updated_In_Constructor()
    {
      var spec = new ContactByNameMatchSpec("tes");

      var contact = new Contact() { ContactId = 1, Name = "tess" };
      spec.IsSatisfiedBy(contact).Should().BeTrue();

      contact = new Contact() { ContactId = 2, Name = "test" };
      spec.IsSatisfiedBy(contact).Should().BeTrue();

      contact = new Contact() { ContactId = 1, Name = "ben" };
      spec.IsSatisfiedBy(contact).Should().BeFalse();
    }
  }

  public class ContactByNameMatchSpec : Specification<Contact>
  {
    public ContactByNameMatchSpec(string name)
        : base(p => p.Name.Contains(name))
    {
    }
  }

}