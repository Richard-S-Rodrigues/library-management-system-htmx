namespace LibraryManagementSystemHtmx.Models;

public class IssuedBook 
{
  public int? Id { get; set; }
  public int MemberId { get; set; }
  public int BookId { get; set; }
  public DateTime IssuedDate { get; set; } = DateTime.UtcNow;
  public DateTime ReturnDate { get; set; }
}