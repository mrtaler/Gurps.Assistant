using System;

namespace Gurps.Assistant.Domain.Repository.Interfaces
{
  public interface IEntity
  {
    object Id { get; set; }
    DateTime CreatedOn { get; set; }
    object CreatedBy { get; set; }
    DateTime? ModifiedOn { get; set; }
    object ModifiedBy { get; set; }
    DateTime? DeletedOn { get; set; }
    object DeletedBy { get; set; }
  }

  public interface IEntity<TId> : IEntity where TId : struct
  {
    new TId Id { get; set; }
    new TId CreatedBy { get; set; }
    new TId? ModifiedBy { get; set; }
    new TId? DeletedBy { get; set; }
  }
}