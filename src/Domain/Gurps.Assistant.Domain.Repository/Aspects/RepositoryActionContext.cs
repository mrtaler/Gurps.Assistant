﻿using Gurps.Assistant.Domain.Repository.Interfaces.Repository;

namespace Gurps.Assistant.Domain.Repository.Aspects
{
  public class RepositoryActionContext<T, TKey>
      where T : class
  {
    public RepositoryActionContext(IRepository<T, TKey> repository)
    {
      Repository = repository;
    }

    public IRepository<T, TKey> Repository { get; set; }
  }
}