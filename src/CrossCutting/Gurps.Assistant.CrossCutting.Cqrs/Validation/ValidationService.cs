using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Gurps.Assistant.CrossCutting.Cqrs.Commands;

namespace Gurps.Assistant.CrossCutting.Cqrs.Validation
{
  public class ValidationService : IValidationService
  {
    private readonly IValidationProvider _validationProvider;

    public ValidationService(IValidationProvider validationProvider)
    {
      _validationProvider = validationProvider;
    }

    /// <inheritdoc />
    public async Task ValidateAsync(ICommand command)
    {
      CheckCommand(command);

      var validationResponse = await _validationProvider.ValidateAsync(command);

      if (!validationResponse.IsValid)
        throw new ValidationException(BuildErrorMessage(validationResponse.Errors));
    }

    /// <inheritdoc />
    public void Validate(ICommand command)
    {
      CheckCommand(command);

      var validationResponse = _validationProvider.Validate(command);

      if (!validationResponse.IsValid)
        throw new ValidationException(BuildErrorMessage(validationResponse.Errors));
    }

    private static string BuildErrorMessage(IEnumerable<ValidationError> errors)
    {
      var errorsText = errors.Select(x => $"\r\n - {x.ErrorMessage}").ToArray();
      return $"Validation failed: {string.Join("", errorsText)}";
    }

    public void CheckCommand(ICommand command)
    {
      if (command == null)
        throw new ArgumentNullException(nameof(command));
    }

  }
}
