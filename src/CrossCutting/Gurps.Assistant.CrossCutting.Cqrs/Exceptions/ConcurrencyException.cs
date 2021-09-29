using System;

namespace Gurps.Assistant.CrossCutting.Cqrs.Exceptions
{
  public class ConcurrencyException : Exception
  {
    public ConcurrencyException(Guid aggregateRootId, int expectedVersion, int actualVersion)
        : base(BuildErrorMessage(aggregateRootId, expectedVersion, actualVersion))
    {
    }

    private static string BuildErrorMessage(Guid aggregateRootId, int expectedVersion, int actualVersion)
    {
      return "Concurrency Exception" +
             $" | AggregateRootId: {aggregateRootId}" +
             $" | Expected version: {expectedVersion}" +
             $" | Actual version: {actualVersion}";
    }
  }
}