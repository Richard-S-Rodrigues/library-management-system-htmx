using LibraryManagementSystemHtmx.Models;

namespace LibraryManagementSystemHtmx.Services.BookService;

public interface IBookService : ICrudService<Book>
{
  Task<IssuedBook> IssueBook(int memberId, int bookId, DateTime returnDate);
}