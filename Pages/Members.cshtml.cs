using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibraryManagementSystemHtmx.Models;
using LibraryManagementSystemHtmx.Services.MemberService;

namespace LibraryManagementSystemHtmx.Pages;

[IgnoreAntiforgeryToken]
public class MembersModel : PageModel
{
    private readonly IMemberService _memberService;
    public IList<Member> Members { get; set; } = new List<Member>(); 

    public TableData MemberTable { get; set; } = new();

    [BindProperty]
    public Member CurrentSelectedMember { get; set; } = new();

    public MembersModel(IMemberService memberService)
    {
        _memberService = memberService;
    }

    public async Task<IActionResult> OnPostSaveMember()
    {
        if (ModelState.IsValid)
        {
            if (CurrentSelectedMember.Id is null) 
            { 
                CurrentSelectedMember = await _memberService.Create(CurrentSelectedMember);
            }
            else 
            {
                await _memberService.Update(CurrentSelectedMember.Id.Value, CurrentSelectedMember);
            }
            
            return RedirectToPage();
        }

        return Page();
    }

    public async Task OnGetAsync()
    {
        MemberTable = new TableData("Name", "Email", "Address", "MaxBookLimit");
        Members = await _memberService.GetAll();
        
        foreach (var Member in Members)
        {
            MemberTable.AddRow(Member.Id!.Value, Member);
        }
    }

    public async Task<IActionResult> OnGetOpenEditModal(int? id)
    {    
        if (id is null)
        {
            return Partial("~/Pages/MemberEdit.cshtml", this);
        }
        
        CurrentSelectedMember = await _memberService.Get(id.Value);
        
        if (CurrentSelectedMember is null)
        {
            return Partial("~/Pages/MemberEdit.cshtml", this);
        }

        return Partial("~/Pages/MemberEdit.cshtml", this);
    }

    public async Task OnPostDeleteAction(int rowId)
    {
        await _memberService.Delete(rowId);
        await OnGetAsync();
    }

    public async Task<IActionResult> OnPostSearch(string searchQuery)
    {
        await OnGetAsync();
        if (string.IsNullOrEmpty(searchQuery))
        {
            return Partial("~/Pages/Shared/_Rows.cshtml", MemberTable);
        }

        IList<Member> rows = MemberTable.Rows.Select(row => (Member)row.Value).ToList(); 
        var Results = rows.Where(
            row => 
                row.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                row.Email.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                row.Address.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)  
        ).ToList();

        TableData updatedTable = MemberTable;
        updatedTable.RemoveAllRows();

        foreach (var row in Results)
        {
            updatedTable.AddRow(row.Id!.Value, row);
        }

        return Partial("~/Pages/Shared/_Rows.cshtml", updatedTable);
    }
}
