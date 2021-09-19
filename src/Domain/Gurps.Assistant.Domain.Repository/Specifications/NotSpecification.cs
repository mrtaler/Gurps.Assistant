namespace Gurps.Assistant.Domain.Repository.Specifications
{
  public class NotSpecification<T> : Specification<T>
  {
    public NotSpecification(ISpecification<T> specification) : base(specification.Predicate.Not())
    {
    }
  }
}