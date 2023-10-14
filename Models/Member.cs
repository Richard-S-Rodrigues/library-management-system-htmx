using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystemHtmx.Models;

public class Member : BaseEntity
{
  public string Name { get; set; }
  [EmailAddress]
  public string Email { get; set; }
  public string Address { get; set; }
  public int MaxBookLimit { get; set; }
}