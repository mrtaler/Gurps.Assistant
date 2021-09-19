using Gurps.Assistant.Domain.Repository.MongoDb;

namespace Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects.Mongo
{

  public class ContactTypeMongo : MongoDbEntity
  {
    public int ContactTypeId { get; set; }
    public string Name { get; set; }
    public string Abbreviation { get; set; }
  }
}