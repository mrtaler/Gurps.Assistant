using Gurps.Assistant.Domain.Repository.Attributes;
using Gurps.Assistant.Domain.Repository.MongoDb;

namespace Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects.Mongo
{

  [RepositoryLogging]
  public class EmailAddressMongo : MongoDbEntity
  {
    public int EmailAddressId { get; set; }
    public int ContactId { get; set; }
    public string Label { get; set; }
    public string Email { get; set; }
  }
}