using System;
using System.Reflection;
using Gurps.Assistant.Domain.Repository.EntityFrameworkCore.SharpRepository;
using Gurps.Assistant.Domain.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gurps.Assistant.Domain.Repository.EntityFrameworkCore
{
  public class DbCoreContextFactory<TContext> : IDataContextFactory<TContext>
        where TContext : DbContext, ICoreDbContext
  {
    private readonly DbContextOptions<TContext> options;
    public DbCoreContextFactory(DbContextOptions<TContext> options)
    {
      this.options = options;
    }

    private TContext context;

    public TContext GetContext()
    {
      context ??= Activator.CreateInstance(typeof(TContext), BindingFlags.Default, null, new object[] { options }, null, null) as TContext;
      // _context?.Database.EnsureCreated();
      return context;
    }
  }
}
