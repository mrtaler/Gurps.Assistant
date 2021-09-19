using System;
using MongoDB.Bson.Serialization;

namespace Gurps.Assistant.Domain.Repository.MongoDb
{
  public class GuidIdGenerator : IIdGenerator
  {
    public object GenerateId(object container, object document)
    {
      return Guid.NewGuid();
    }

    public bool IsEmpty(object id)
    {
      var guid = Guid.Empty;
      if (id != null)
      {
        Guid.TryParse(id.ToString(), out guid);
      }
      return Guid.Empty.Equals(guid);
    }
  }
}
