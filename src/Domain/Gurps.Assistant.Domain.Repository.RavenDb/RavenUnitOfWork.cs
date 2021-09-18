using System;
using Autofac;
using Gurps.Assistant.Domain.Repository.Interfaces;
using Raven.Client.Documents.Session;

namespace Gurps.Assistant.Domain.Repository.RavenDb
{
  //  public abstract class UnitOfWorkBase<TContext>
  public partial class RavenUnitOfWork
      : UnitOfWorkBase<IDocumentSession, RavenDbContextFactory>
  {
    private ILifetimeScope scope;
    public RavenUnitOfWork(ILifetimeScope container, RavenDbContextFactory dataContextFactory)
    {
      scope = container.BeginLifetimeScope();
      DataContextFactory = dataContextFactory;
    }

    private bool isDisposed;

    /// <inheritdoc />
    public override RavenDbContextFactory DataContextFactory { get; }

    /// <inheritdoc />
    protected override IRepositoryFactory CreateRepositoryFactory()
    {
      return new RavenDbRepositoryFactory(DataContextFactory);
    }

    /// <inheritdoc />
    public override int? SaveChanges()
    {
      try
      {
        DataContextFactory.GetContext().SaveChanges();
        return 1;
      }
      catch
      {
        return 0;
      }
    }
    public override void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
    private void Dispose(bool disposing)
    {
      if (isDisposed)
        return;

      if (disposing)
      {
        //ContextSource.Client.;
      }
      // Free any unmanaged objects here.
      //
      isDisposed = true;
    }

    ~RavenUnitOfWork()
    {
      Dispose(false);
    }
  }
}
