using Gurps.Assistant.Domain.Repository.Attributes;
using Gurps.Assistant.Domain.Repository.MongoDb;

namespace Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects.Mongo
{

  [RepositoryLogging]
  public class PhoneNumberMongo : MongoDbEntity
  {
    public int PhoneNumberId { get; set; }
    public int ContactId { get; set; }
    public string Label { get; set; }
    public string Number { get; set; }
  }
}