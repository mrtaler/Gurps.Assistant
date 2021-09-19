using System.Linq.Expressions;

namespace Gurps.Assistant.Domain.Repository.UnitTests.TestObjects.Assert
{
  public static class ReflectionExtensions
  {
    public static string GetPropertyName(this LambdaExpression expression)
    {
      var memberExpression = expression.Body is UnaryExpression unaryExpression
                                              ? (MemberExpression)unaryExpression.Operand
                                              : (MemberExpression)expression.Body;

      return memberExpression.Member.Name;
    }
  }
}