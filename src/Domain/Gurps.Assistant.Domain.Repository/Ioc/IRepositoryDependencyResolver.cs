using System;

namespace Gurps.Assistant.Domain.Repository.Ioc
{
  public interface IRepositoryDependencyResolver
  {
    T Resolve<T>();
    object Resolve(Type type);
  }
}