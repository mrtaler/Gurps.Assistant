using Gurps.Assistant.Domain.Repository.Aspects;
using System.Threading;

namespace Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects
{
  internal sealed class SpecificAuditAttribute : AuditAttributeMock
  {
    public override void OnGetExecuted<T, TKey, TResult>(RepositoryGetContext<T, TKey, TResult> context)
    {
      Thread.Sleep(50);
      base.OnGetExecuted(context);
    }
  }
}