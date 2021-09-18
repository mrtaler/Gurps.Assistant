﻿using System.Threading;

namespace Gurps.Assistant.Domain.Repository.Caching
{
  public class SingleServerCachePrefixManager : ICachePrefixManager
  {
    private static int _counter = 1;

    public int Counter
    {
      get { return _counter; }
    }

    public void IncrementCounter()
    {
      Interlocked.Increment(ref _counter);
    }
  }
}
