using System.Linq;
using FluentAssertions;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects.Assert;
using Xunit;

namespace Gurps.Assistant.Domain.Repository.UnitTests.FetchStrategies
{
  public class FetchStrategyTests : TestBase
  {
    [Fact]
    public void FetchStrategy_May_Include_Multiple_References()
    {
      var strategy = new GenericFetchStrategy<Contact>()
                     .Include(p => p.EmailAddresses)
                     .Include(p => p.PhoneNumbers);

      strategy.IncludePaths.Should().Contain("EmailAddresses");
      strategy.IncludePaths.Should().Contain("PhoneNumbers");
      strategy.IncludePaths.Count().Should().Be(2);
    }

    [Fact]
    public void FetchStrategy_May_Include_String_Property_Names()
    {
      // This is a non-sense example because the Email property is not another table, but it works the exact same
      var strategy = new GenericFetchStrategy<Contact>()
                     .Include("EmailAddresses")
                     .Include("PhoneNumbers");

      strategy.IncludePaths.Should().Contain("EmailAddresses");
      strategy.IncludePaths.Should().Contain("PhoneNumbers");
      strategy.IncludePaths.Count().Should().Be(2);
    }

    [Fact]
    public void FetchStrategy_May_Include_Multiple_Levels()
    {
      // This is a non-sense example because the Email property is not another table, but it works the exact same
      var strategy = new GenericFetchStrategy<Contact>()
          .Include(p => p.EmailAddresses.Select(e => e.Email));

      strategy.IncludePaths.Should().Contain("EmailAddresses.Email");
    }

    [Fact]
    public void FetchStrategy_May_Include_Multiple_Levels_First_Syntax()
    {
      // This is a non-sense example because the Email property is not another table, but it works the exact same
      var strategy = new GenericFetchStrategy<Contact>()
          .Include(p => p.EmailAddresses.First().Email);

      strategy.IncludePaths.Should().Contain("EmailAddresses.Email");
    }
  }
}
