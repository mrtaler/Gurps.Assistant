using System;
using MongoDB.Bson.Serialization.Attributes;
using Gurps.Assistant.Domain.Repository.Interfaces;

namespace Gurps.Assistant.Domain.Repository.MongoDb
{
  public abstract class MongoDbEntity : IEntity<Guid>
  {
    object IEntity.Id
    {
      get => Id;
      set => Id = (Guid)value;
    }

    public Guid CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public Guid? DeletedBy { get; set; }

    [BsonId(IdGenerator = typeof(GuidIdGenerator))]
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    object IEntity.CreatedBy
    {
      get => CreatedBy;
      set => CreatedBy = (Guid)value;
    }

    public DateTime? ModifiedOn { get; set; }
    object IEntity.ModifiedBy
    {
      get => ModifiedBy;
      set => ModifiedBy = (Guid)value;
    }

    public DateTime? DeletedOn { get; set; }
    object IEntity.DeletedBy
    {
      get => DeletedBy;
      set => DeletedBy = (Guid)value;
    }
  }
}
