using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibraryManagementSystemHtmx.Models;

namespace LibraryManagementSystemHtmx.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private IList<Book> _books;
    public IList<Book> books
    {
        get 
        {
            if (_books == null)
            {
                _books =  new List<Book>() 
                {
                    new() 
                    {
                        Id = 1,
                        Title = "Test title 1",
                        Description = "Test description 1"
                    },
                    new() 
                    {
                        Id = 2,
                        Title = "Test title 2",
                        Description = "Test description 2"
                    }
                };
            }
            return _books;
        }
        private set 
        {
            _books = value;
        }
    } 

    private TableData _bookTable;
    public TableData bookTable 
    {
        get
        {
            if (_bookTable == null)
            {
                _bookTable = new TableData("Title", "Description");

                foreach (var book in books)
                {
                    _bookTable.AddRow(book.Id, book);
                }
            }
            return _bookTable;
        }
        private set 
        {
            _bookTable = value;
        }
    }

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }

    public IActionResult OnGetOpenEditModal(int? rowId)
    {    
        if (rowId is null)
        {
            return Partial("~/Pages/BookEdit.cshtml", new Book());
        }
        
        var row = bookTable.GetRowById(rowId.Value);
        if (row is null)
        {
            return Partial("~/Pages/BookEdit.cshtml", new Book());
        }
        return Partial("~/Pages/BookEdit.cshtml", (Book)row.Value);
    }

    public void OnPostDeleteAction(int rowId)
    {
        bookTable.RemoveRow(rowId);
    }

    public IActionResult OnGetSearch(string searchQuery)
    {
        IList<Book> rows = bookTable.Rows.Select(row => (Book)row.Value).ToList(); 
        var Results = string.IsNullOrEmpty(searchQuery) 
            ? rows 
            : rows.Where(
                row => 
                    row.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) || 
                    row.Description.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)
            ).ToList();

        TableData updatedTable = bookTable;
        updatedTable.RemoveAllRows();

        foreach (var row in Results)
        {
            updatedTable.AddRow(row.Id, row);
        }

        return Partial("~/Pages/Shared/_Rows.cshtml", updatedTable);
    }
}
