using Gurps.Assistant.CrossCutting.Cqrs.Exceptions;
using System;

namespace Gurps.Assistant.CrossCutting.Cqrs.Domain
{
  public class VersionService : IVersionService
  {
    /// <inheritdoc />
    /// <exception cref="T:RolePlayedGamesHelper.Cqrs.Kledex.Exceptions.ConcurrencyException"></exception>
    public int GetNextVersion(Guid aggregateRootId, int currentVersion, int? expectedVersion)
    {
      if (expectedVersion.HasValue && expectedVersion.Value > 0 && expectedVersion.Value != currentVersion)
      {
        throw new ConcurrencyException(aggregateRootId, expectedVersion.Value, currentVersion);
      }

      return currentVersion + 1;
    }
  }
}