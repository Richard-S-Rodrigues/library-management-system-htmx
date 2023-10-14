using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibraryManagementSystemHtmx.Models;
using LibraryManagementSystemHtmx.Services.BookService;

namespace LibraryManagementSystemHtmx.Pages;

public class IndexModel : PageModel
{
    IEnumerable<Book> mockBooks = new List<Book>() 
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
    private readonly IBookService _bookService;
    private IEnumerable<Book> _books;
    public IEnumerable<Book> books
    {
        get 
        {
            if (_books == null)
            {
                _books =  mockBooks;
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
                    _bookTable.AddRow(book.Id!.Value, book);
                }
            }
            return _bookTable;
        }
        private set 
        {
            _bookTable = value;
        }
    }

    public IndexModel(IBookService bookService)
    {
        _bookService = bookService;
    }

    public void SaveBook()
    {
        
    }

    public async void OnGet()
    {
        _books = await _bookService.GetAll();
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
            updatedTable.AddRow(row.Id!.Value, row);
        }

        return Partial("~/Pages/Shared/_Rows.cshtml", updatedTable);
    }
}
