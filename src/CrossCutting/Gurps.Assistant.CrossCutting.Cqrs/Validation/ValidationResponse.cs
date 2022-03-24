using System.Collections.Generic;

namespace Gurps.Assistant.CrossCutting.Cqrs.Validation
{
  public class ValidationResponse
  {
    public IList<ValidationError> Errors { get; set; } = new List<ValidationError>();
    public bool IsValid => Errors.Count == 0;
  }
}
