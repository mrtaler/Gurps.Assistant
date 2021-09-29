using Gurps.Assistant.CrossCutting.Cqrs.Domain;

namespace Gurps.Assistant.CrossCutting.Cqrs.Store.RavenDb.Documents.Factories
{
  public interface ICommandDocumentFactory
  {
    CommandDocument CreateCommand(IDomainCommand command);
  }
}