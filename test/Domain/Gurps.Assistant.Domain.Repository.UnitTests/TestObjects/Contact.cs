using System.Collections.Generic;
using Gurps.Assistant.Domain.Repository.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Gurps.Assistant.Domain.Repository.UnitTests.TestObjects
{
  [RepositoryLogging]
  public class Contact
  {
    public int ContactId { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public int ContactTypeId { get; set; } // for partitioning on

    public List<EmailAddress> EmailAddresses { get; set; }
    public List<PhoneNumber> PhoneNumbers { get; set; }

    public ContactType ContactType { get; set; }

    public byte[] Image { get; set; }
  }
  public class TestObjectEntities : DbContext
  {
    /* public TestObjectEntities(string connectionString) : base(connectionString)
     {

     }*/

    public DbSet<Contact> Contacts { get; set; }
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }
    public DbSet<EmailAddress> EmailAddresses { get; set; }
    public DbSet<TripleCompoundKeyItemInts> TripleCompoundKeyItems { get; set; }
  }
}