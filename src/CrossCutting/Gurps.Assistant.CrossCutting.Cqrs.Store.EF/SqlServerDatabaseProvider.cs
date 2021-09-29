﻿using Microsoft.EntityFrameworkCore;

namespace Gurps.Assistant.CrossCutting.Cqrs.Store.EF
{
  public class SqlServerDatabaseProvider : IDatabaseProvider
  {
    public DomainDbContext CreateDbContext(string connectionString)
    {
      var optionsBuilder = new DbContextOptionsBuilder<DomainDbContext>();
      optionsBuilder.UseSqlServer(connectionString);

      return new DomainDbContext(optionsBuilder.Options);
    }
  }
}