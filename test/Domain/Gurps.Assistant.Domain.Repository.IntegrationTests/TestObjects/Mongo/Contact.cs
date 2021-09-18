using Gurps.Assistant.Domain.Repository.MongoDb;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects.Mongo
{
  [BsonIgnoreExtraElements]
  public class ContactMongo : MongoDbEntity
  {
    public int ContactId { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public int ContactTypeId { get; set; } // for partitioning on
    public List<EmailAddressMongo> EmailAddresses { get; set; }
    public List<PhoneNumberMongo> PhoneNumbers { get; set; }
    public ContactTypeMongo ContactType { get; set; }
    public byte[] Image { get; set; }
  }
}