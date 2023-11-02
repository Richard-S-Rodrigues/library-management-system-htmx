namespace LibraryManagementSystemHtmx.Models;

public class Book : BaseEntity
{
  public string Isbn { get; set; } = string.Empty;
  public string Title { get; set; } = string.Empty;
  public string Author { get; set; } = string.Empty;
  public string? Description { get; set; } = null!;
}