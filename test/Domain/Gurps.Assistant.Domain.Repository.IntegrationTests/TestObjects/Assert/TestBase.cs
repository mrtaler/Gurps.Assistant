using Gurps.Assistant.Domain.Repository.IntegrationTests.Common;
using Moq;

namespace Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects.Assert
{
  [LogTestName]
  public abstract class TestBase
  {
    protected TestBase()
    {
      TestHelper = new TestHelper();
    }

    public ITestHelper TestHelper { get; }

    protected static T N<T>() where T : class
    {
      return default;
    }

    /// <summary>
    /// Create a mock
    /// </summary>
    /// <typeparam name="T">Type to be mocked</typeparam>
    /// <param name="argumentsForConstructor">Constructor arguments</param>
    /// <returns>T</returns>
    protected static T M<T>(params object[] argumentsForConstructor) where T : class
    {
      var mock = new MockRepository(MockBehavior.Default).Create<T>(argumentsForConstructor);
      return mock.Object;
    }

    /// <summary>
    /// Create a partial mock
    /// </summary>
    /// <typeparam name="T">Type to be partially mocked</typeparam>
    /// <param name="argumentsForConstructor">Constructor arguments</param>
    /// <returns>T</returns>
    protected static T Pm<T>(params object[] argumentsForConstructor) where T : class
    {
      return M<T>(argumentsForConstructor);
    }

    /// <summary>
    /// Create a stub
    /// </summary>
    /// <typeparam name="T">Type to be stubbed</typeparam>
    /// <param name="argumentsForConstructor">Constructor arguments</param>
    /// <returns>T</returns>
    protected static T S<T>(params object[] argumentsForConstructor) where T : class
    {
      return M<T>(argumentsForConstructor);
    }
  }
}
