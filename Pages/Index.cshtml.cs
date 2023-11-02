using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibraryManagementSystemHtmx.Models;
using LibraryManagementSystemHtmx.Services.BookService;

namespace LibraryManagementSystemHtmx.Pages;

[IgnoreAntiforgeryToken]
public class IndexModel : PageModel
{
    private readonly IBookService _bookService;
    public IList<Book> books { get; set; } = new List<Book>(); 

    public TableData bookTable { get; set; } = new();

    [BindProperty]
    public Book currentSelectedBook { get; set; } = new();

    public IndexModel(IBookService bookService)
    {
        _bookService = bookService;
    }

    public async Task<IActionResult> OnPostSaveBook()
    {
        if (ModelState.IsValid)
        {
            if (currentSelectedBook.Id is null) 
            { 
                currentSelectedBook = await _bookService.Create(currentSelectedBook);
            }
            else 
            {
                await _bookService.Update(currentSelectedBook.Id.Value, currentSelectedBook);
            }
            
            return RedirectToPage();
        }

        return Page();
    }

    public async Task OnGetAsync()
    {
        bookTable = new TableData("Isbn", "Title", "Author", "Description");
        books = await _bookService.GetAll();
        
        foreach (var book in books)
        {
            bookTable.AddRow(book.Id!.Value, book);
        }
    }

    public async Task<IActionResult> OnGetOpenEditModal(int? id)
    {    
        if (id is null)
        {
            return Partial("~/Pages/BookEdit.cshtml", this);
        }
        
        currentSelectedBook = await _bookService.Get(id.Value);
        
        if (currentSelectedBook is null)
        {
            return Partial("~/Pages/BookEdit.cshtml", this);
        }

        return Partial("~/Pages/BookEdit.cshtml", this);
    }

    public async Task OnPostDeleteAction(int rowId)
    {
        await _bookService.Delete(rowId);
        await OnGetAsync();
    }

    public async Task<IActionResult> OnPostSearch(string searchQuery)
    {
        await OnGetAsync();
        if (string.IsNullOrEmpty(searchQuery))
        {
            return Partial("~/Pages/Shared/_Rows.cshtml", bookTable);
        }

        IList<Book> rows = bookTable.Rows.Select(row => (Book)row.Value).ToList(); 
        var Results = rows.Where(
            row => 
                row.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                row.Isbn.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                row.Author.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) || 
                row.Description is not null && row.Description.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) 
        ).ToList();

        TableData updatedTable = bookTable;
        updatedTable.RemoveAllRows();

        foreach (var row in Results)
        {
            updatedTable.AddRow(row.Id!.Value, row);
        }

        return Partial("~/Pages/Shared/_Rows.cshtml", updatedTable);
    }
}
