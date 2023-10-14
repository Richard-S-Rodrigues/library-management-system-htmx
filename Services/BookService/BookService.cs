using LibraryManagementSystemHtmx.Data.DbAccess;
using LibraryManagementSystemHtmx.Models;

namespace LibraryManagementSystemHtmx.Services.BookService;

public class BookService : IBookService
{
  private readonly IDbAccess _dbAccess;

  public BookService(IDbAccess dbAccess)
  {
    _dbAccess = dbAccess;
  }
  
  public async Task<Book> Create(Book request)
  {
    Book data = new()
    {
      Isbn = request.Isbn,
      Title = request.Title,
      Author = request.Author,
      Description = request.Description
    };

    var generatedId = await _dbAccess.SaveData<Book, dynamic>
    (
      @"
        INSERT INTO book (isbn, title, author, description, created_at, updated_at)
        VALUES (@Isbn, @Title, @Author, @Description, @CreatedAt, @UpdatedAt)
      ",
      data
    );
    data.Id = generatedId;

    return data;
  }

  public async Task Delete(int id)
  {
    await _dbAccess.SaveData<Book, dynamic>("DELETE FROM book WHERE id = @Id", new { Id = id });
  }

  public async Task<Book> Get(int id)
  {
    var result = await _dbAccess.GetData<Book, dynamic>
    (
      @"
        SELECT
          id AS Id,
          isbn AS Isbn,
          title AS Title,
          author AS Author,
          description AS Description,
          created_at AS CreatedAt,
          updated_at AS UpdatedAt
        FROM book WHERE id = @Id
      ",
      new { Id = id }
    );

    return result.FirstOrDefault()!;
  }

  public async Task<IEnumerable<Book>> GetAll()
  {
    var result = await _dbAccess.GetData<Book, dynamic>
    (
      @"
        SELECT
          id AS Id,
          isbn AS Isbn,
          title AS Title,
          author AS Author,
          description AS Description,
          created_at AS CreatedAt,
          updated_at AS UpdatedAt
        FROM book
      ", new {}
    );
    return result.ToList();
  }

  public async Task<Book> Update(int id, Book request)
  {
    Book data = new()
    {
      Id = id,
      Isbn = request.Isbn,
      Title = request.Title,
      Author = request.Author,
      Description = request.Description,
      CreatedAt = request.CreatedAt
    };

    await _dbAccess.SaveData<Book, dynamic>
    (
      @"
        UPDATE book b SET
          isbn = @Isbn,
          title = @Title,
          author = @Author,
          description = @Description,
          created_at = @CreatedAt,
          updated_at = @UpdatedAt
        WHERE b.id = @Id
      ", 
      data
    );
    return data;
  }

  public async Task<IssuedBook> IssueBook(int memberId, int bookId, DateTime returnDate)
  {
    IssuedBook data = new()
    {
      MemberId = memberId,
      BookId = bookId,
      ReturnDate = returnDate
    };

    var generatedId = await _dbAccess.SaveData<IssuedBook, dynamic>
    (
      @"
        INSERT INTO issued_book (member_id, book_id, issued_date, return_date)
        VALUES (@MemberId, @BookId, @IssuedDate, @ReturnDate)
      ",
      data
    );
    data.Id = generatedId;

    return data;
  }
}