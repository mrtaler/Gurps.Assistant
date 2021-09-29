using System;

namespace Gurps.Assistant.CrossCutting.Cqrs.Exceptions
{
  public class ValidationException : Exception
  {
    public ValidationException(string errorMessage) : base(errorMessage)
    {
    }
  }
}
