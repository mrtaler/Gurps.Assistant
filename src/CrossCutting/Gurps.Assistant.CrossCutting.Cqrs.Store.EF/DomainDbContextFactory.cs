using System;
using Gurps.Assistant.CrossCutting.Cqrs.Dependencies;
using Microsoft.Extensions.Configuration;

namespace Gurps.Assistant.CrossCutting.Cqrs.Store.EF
{
  public class DomainDbContextFactory : IDomainDbContextFactory
  {
    private readonly IResolver resolver;
    private readonly string connectionString;

    public DomainDbContextFactory(IResolver resolver, IConfiguration configuration)
    {
      this.resolver = resolver;
      connectionString = configuration.GetConnectionString(Consts.DomainStoreConnectionString);
    }

    public DomainDbContext CreateDbContext()
    {
      var dataProvider = resolver.Resolve<IDatabaseProvider>();

      if (dataProvider == null)
        throw new ArgumentNullException("Domain database provider not found.");

      return dataProvider.CreateDbContext(connectionString);
    }
  }
}