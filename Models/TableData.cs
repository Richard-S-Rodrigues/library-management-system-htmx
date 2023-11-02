namespace LibraryManagementSystemHtmx.Models;

public class TableData
{
  public List<string> Columns;
  public IList<Row> Rows { get; set; }
  public string SearchQuery { get; set; }

  public TableData(params string[] columnNames)
  {
    Columns = columnNames.ToList();
    Rows = new List<Row>();
    SearchQuery = string.Empty;
  }

  public IEnumerable<string> GetRowValues(int rowId)
  {
    var row = GetRowById(rowId);
    if (row is null) return new List<string>();
    
    return Columns.Select(column => {
      var prop = row.Value.GetType().GetProperty(column);
      var value = prop!.GetValue(row.Value);
      if (value is null) return null;
      return value.ToString();
      }).ToList()!;
  }

  public void AddRow(int id, object value)
  {
    Rows.Add(
      new Row(id, value) 
    );
  }

  public void RemoveRow(int id)
  {
    var row = GetRowById(id);
    
    if (row is null) return;

    Rows.Remove(row);
  }

  public void RemoveAllRows()
  {
    Rows.Clear();
  }

  public Row? GetRowById(int id)
  {
    return Rows.FirstOrDefault(row => row.Id == id);
  }
}