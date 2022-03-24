using System;

namespace Gurps.Assistant.CrossCutting.Cqrs.Exceptions
{
  [Serializable]
  public class QueryException : Exception
  {
    public QueryException(string errorMessage) : base(errorMessage)
    {
    }
  }
}
