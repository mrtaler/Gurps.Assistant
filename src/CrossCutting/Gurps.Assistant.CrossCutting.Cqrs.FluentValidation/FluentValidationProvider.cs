using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Gurps.Assistant.CrossCutting.Cqrs.Commands;
using Gurps.Assistant.CrossCutting.Cqrs.Dependencies;
using Gurps.Assistant.CrossCutting.Cqrs.Validation;

namespace Gurps.Assistant.CrossCutting.Cqrs.FluentValidation
{
  public class FluentValidationProvider : IValidationProvider
  {
    private readonly IHandlerResolver handlerResolver;

    public FluentValidationProvider(IHandlerResolver handlerResolver)
    {
      this.handlerResolver = handlerResolver;
    }

    public async Task<ValidationResponse> ValidateAsync(ICommand command)
    {
      var validator = handlerResolver.ResolveHandler(command, typeof(IValidator<>));
      var validateMethod = validator.GetType().GetMethod("ValidateAsync", new[] { command.GetType(), typeof(CancellationToken) });
      var validationResult = await (Task<ValidationResult>)validateMethod.Invoke(validator, new object[] { command, default(CancellationToken) });

      return BuildValidationResponse(validationResult);
    }

    public ValidationResponse Validate(ICommand command)
    {
      var validator = handlerResolver.ResolveHandler(command, typeof(IValidator<>));
      var validateMethod = validator.GetType().GetMethod("Validate", new[] { command.GetType() });
      var validationResult = (ValidationResult)validateMethod.Invoke(validator, new object[] { command });

      return BuildValidationResponse(validationResult);
    }

    private static ValidationResponse BuildValidationResponse(ValidationResult validationResult)
    {
      return new ValidationResponse
      {
        Errors = validationResult.Errors.Select(failure => new ValidationError
        {
          PropertyName = failure.PropertyName,
          ErrorMessage = failure.ErrorMessage
        }).ToList()
      };
    }
  }

}
