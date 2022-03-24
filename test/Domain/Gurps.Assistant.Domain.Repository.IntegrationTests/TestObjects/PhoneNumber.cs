using Gurps.Assistant.Domain.Repository.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects
{
  [BsonIgnoreExtraElements]
  [RepositoryLogging]
  public class PhoneNumber
  {
    public int PhoneNumberId { get; set; }
    public int ContactId { get; set; }
    public string Label { get; set; }
    public string Number { get; set; }
  }
}