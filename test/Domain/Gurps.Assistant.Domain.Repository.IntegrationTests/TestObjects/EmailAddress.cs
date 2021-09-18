using Gurps.Assistant.Domain.Repository.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects
{
  [BsonIgnoreExtraElements]
  [RepositoryLogging]
  public class EmailAddress
  {
    public int EmailAddressId { get; set; }
    public int ContactId { get; set; }
    public string Label { get; set; }
    public string Email { get; set; }
  }
}