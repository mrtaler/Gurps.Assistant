﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Gurps.Assistant.CrossCutting.Cqrs.Store.EF
{
  public class MigrationsDbContextFactory : IDesignTimeDbContextFactory<DomainDbContext>
  {
    public DomainDbContext CreateDbContext(string[] args)
    {
      var builder = new DbContextOptionsBuilder<DomainDbContext>();
      builder.UseSqlServer("UsedForMigrationsOnlyUntilClassLibraryBugIsFixed");

      return new DomainDbContext(builder.Options);
    }
  }
}