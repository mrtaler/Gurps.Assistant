﻿using System;

namespace Gurps.Assistant.CrossCutting.Cqrs.Domain
{
  public abstract class Entity : IEntity
  {
    public Guid Id { get; protected set; }

    protected Entity()
    {
      Id = Guid.NewGuid();
    }

    protected Entity(Guid id)
    {
      if (id == Guid.Empty)
        id = Guid.NewGuid();

      Id = id;
    }
  }
}