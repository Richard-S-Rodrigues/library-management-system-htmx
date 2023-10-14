using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibraryManagementSystemHtmx.Models;

namespace LibraryManagementSystemHtmx.Pages;

public class MembersModel : PageModel
{
  private IList<Member> _members;
  public IList<Member> members
  {
      get 
      {
          if (_members == null)
          {
              _members =  new List<Member>() 
              {
                  new() 
                  {
                      Id = 1,
                      Name = "Member 1"
                  },
                  new() 
                  {
                      Id = 2,
                      Name = "Member 2"
                  }
              };
          }
          return _members;
      }
      private set 
      {
          _members = value;
      }
  } 
    
  private TableData _memberTable;
  public TableData memberTable 
  {
      get
      {
          if (_memberTable == null)
          {
              _memberTable = new TableData("Name");

              foreach (var member in members)
              {
                  _memberTable.AddRow(member.Id!.Value, member);
              }
          }
          return _memberTable;
      }
      private set 
      {
          _memberTable = value;
      }
  }

  public MembersModel()
  {
  }

  public void OnGet()
  {
  }

  public IActionResult OnGetOpenEditModal(int? rowId)
  {    
      if (rowId is null)
      {
          return Partial("~/Pages/MemberEdit.cshtml", new Member());
      }
      
      var row = memberTable.GetRowById(rowId.Value);
      if (row is null)
      {
          return Partial("~/Pages/MemberEdit.cshtml", new Member());
      }
      return Partial("~/Pages/MemberEdit.cshtml", (Member)row.Value);
  }

  public void OnPostDeleteAction(int rowId)
  {
    memberTable.RemoveRow(rowId);
  }

  public IActionResult OnGetSearch(string searchQuery)
  {
      IList<Member> rows = memberTable.Rows.Select(row => (Member)row.Value).ToList(); 
      var Results = string.IsNullOrEmpty(searchQuery) 
          ? rows 
          : rows.Where(
              row => 
                  row.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) 
          ).ToList();

      TableData updatedTable = memberTable;
      updatedTable.RemoveAllRows();

      foreach (var row in Results)
      {
          updatedTable.AddRow(row.Id!.Value, row);
      }

      return Partial("~/Pages/Shared/_Rows.cshtml", updatedTable);
  }
}
