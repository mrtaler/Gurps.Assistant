using System.ComponentModel.DataAnnotations;

namespace Gurps.Assistant.Domain.Repository.UnitTests.TestObjects
{
  public class User
  {
    public User()
    {

    }

    [Key]
    public string Id { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }
  }

  public class UserBrief
  {
    public string Id { get; set; }
    public string Email { get; set; }
  }

}