using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibraryManagementSystemHtmx.Models;

namespace LibraryManagementSystemHtmx.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private static int counter = 0;
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
        counter = 0;
    }

    public IActionResult OnPostIncrement()
    {
        return Content($"Counter: <span>{++counter}</span>"); 
    }

    public IActionResult OnGetOpenEditModal(int rowId)
    {
        var row = bookTable.GetRowById(rowId);
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
}
