﻿using Gurps.Assistant.Domain.Repository.Attributes;

namespace Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects
{
  [RepositoryLogging]
  public class ContactType
  {
    public int ContactTypeId { get; set; }
    public string Name { get; set; }
    public string Abbreviation { get; set; }
  }
}