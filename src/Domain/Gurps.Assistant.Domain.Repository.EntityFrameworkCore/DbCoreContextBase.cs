using Gurps.Assistant.Domain.Repository.EntityFrameworkCore.SharpRepository;
using Microsoft.EntityFrameworkCore;

namespace Gurps.Assistant.Domain.Repository.EntityFrameworkCore
{
  public abstract class DbCoreContextBase : DbContext, ICoreDbContext
  {
  }
}