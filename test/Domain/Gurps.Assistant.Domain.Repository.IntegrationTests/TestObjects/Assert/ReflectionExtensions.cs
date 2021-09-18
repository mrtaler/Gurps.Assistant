using System.Linq.Expressions;

namespace Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects.Assert
{
  public static class ReflectionExtensions
  {
    public static string GetPropertyName(this LambdaExpression expression)
    {
      var unaryExpression = expression.Body as UnaryExpression;

      var memberExpression = unaryExpression != null
                                              ? (MemberExpression)unaryExpression.Operand
                                              : (MemberExpression)expression.Body;

      return memberExpression.Member.Name;
    }
  }
}