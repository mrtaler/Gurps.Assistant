using System;
using MongoDB.Driver;
using Gurps.Assistant.Domain.Repository.Interfaces;

namespace Gurps.Assistant.Domain.Repository.MongoDb
{
  public class MongoDbUnitOfWork : UnitOfWorkBase<IMongoClient, MongoDbContextFactory>
  {
    private bool isDisposed;

    public MongoDbUnitOfWork(MongoDbConfiguration configuration)
    {
      DataContextFactory = new MongoDbContextFactory(configuration);
    }
    /// <inheritdoc />
    public override MongoDbContextFactory DataContextFactory { get; }


    /// <inheritdoc />
    protected override IRepositoryFactory CreateRepositoryFactory()
    {
      return new MongoRepositoryFactory(DataContextFactory);
    }

    /// <inheritdoc />
    public override int? SaveChanges()
    {
      try
      {
        var tm = DataContextFactory.GetContext().StartSession();
        tm.CommitTransaction();
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

    // Protected implementation of Dispose pattern.
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
    ~MongoDbUnitOfWork()
    {
      Dispose(false);
    }
  }
}
