﻿namespace Gurps.Assistant.Domain.Repository.UnitTests.TestObjects
{
  [RepositoryLogging]
  public class EmailAddress
  {
    public int EmailAddressId { get; set; }
    public int ContactId { get; set; }
    public string Label { get; set; }
    public string Email { get; set; }
  }
}