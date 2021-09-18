namespace Gurps.Assistant.Domain.Repository.UnitTests.TestObjects
{
  [RepositoryLogging]
  public class PhoneNumber
  {
    public int PhoneNumberId { get; set; }
    public int ContactId { get; set; }
    public string Label { get; set; }
    public string Number { get; set; }
  }
}