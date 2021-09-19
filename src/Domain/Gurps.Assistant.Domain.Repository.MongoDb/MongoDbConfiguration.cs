using Gurps.Assistant.Domain.Repository.Interfaces;

namespace Gurps.Assistant.Domain.Repository.MongoDb
{
  public sealed class MongoDbConfiguration : IDataContextConfiguration
  {
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
  }
}
