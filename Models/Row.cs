namespace LibraryManagementSystemHtmx.Models;

public class Row
{
  public readonly int Id;
  public object Value { get; set; }

  public Row(int id, object value)
  {
    Id = id;
    Value = value;
  }
}