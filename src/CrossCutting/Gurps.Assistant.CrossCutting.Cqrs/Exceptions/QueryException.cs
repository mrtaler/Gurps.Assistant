using System;

namespace Gurps.Assistant.CrossCutting.Cqrs.Exceptions
{
  public class QueryException : Exception
  {
    public QueryException(string errorMessage) : base(errorMessage)
    {
    }
  }
}
