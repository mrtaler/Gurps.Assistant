using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects;
using Xunit;

namespace Gurps.Assistant.Domain.Repository.UnitTests.Spikes
{
  public class GenericClassInheritanceSpikes
  {
    [Fact]
    public void SomeTest()
    {
      _ = new Thing<Contact>();
      _ = new Thing<Contact>();
      _ = new Thing<Contact, string>();
    }
  }

  public interface IThing<T, T2> where T : class
  {
    T DoSomething(T2 t2);
  }

  public interface IThing<T> : IThing<T, int> where T : class
  {
  }

  public class Thing<T> : IThing<T> where T : class
  {
    public T DoSomething(int t2)
    {
      return null;
    }
  }

  public class Thing<T, T2> : IThing<T, T2> where T : class
  {
    public T DoSomething(T2 t2)
    {
      return null;
    }
  }
}
