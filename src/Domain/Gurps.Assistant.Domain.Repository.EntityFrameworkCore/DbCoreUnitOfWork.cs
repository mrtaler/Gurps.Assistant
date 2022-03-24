using System;
using Gurps.Assistant.Domain.Repository.EntityFrameworkCore.SharpRepository;
using Gurps.Assistant.Domain.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gurps.Assistant.Domain.Repository.EntityFrameworkCore
{
  public class DbCoreUnitOfWork<TContext> : UnitOfWorkBase<TContext, DbCoreContextFactory<TContext>>
        where TContext : DbContext, ICoreDbContext
  {
    private readonly DbContextOptions<TContext> options;
    private bool isDisposed;

    public DbCoreUnitOfWork(DbContextOptions<TContext> options, DbCoreContextFactory<TContext> factory)
    {
      this.options = options;
      DataContextFactory = factory;
    }

    /// <inheritdoc />
    public override DbCoreContextFactory<TContext> DataContextFactory { get; }

    /// <inheritdoc />
    public override void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (isDisposed)
        return;

      if (disposing)
      {
        DataContextFactory.GetContext().Dispose();
      }
      // Free any unmanaged objects here.
      //
      isDisposed = true;
    }

    /// <inheritdoc />
    protected override IRepositoryFactory CreateRepositoryFactory()
    {
      return new DbCoreRepositoryFactory<TContext>(DataContextFactory);
    }

    /// <inheritdoc />
    public override int? SaveChanges()
    {
      return DataContextFactory.GetContext().SaveChanges();
    }

    ~DbCoreUnitOfWork()
    {
      Dispose(false);
    }
  }
}
