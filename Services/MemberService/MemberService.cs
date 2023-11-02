using LibraryManagementSystemHtmx.Data.DbAccess;
using LibraryManagementSystemHtmx.Models;

namespace LibraryManagementSystemHtmx.Services.MemberService;

public class MemberService : IMemberService
{
  private readonly IDbAccess _dbAccess;

  public MemberService(IDbAccess dbAccess)
  {
    _dbAccess = dbAccess;
  }
  
  public async Task<Member> Create(Member request)
  {
    Member data = new()
    {
      Name = request.Name,
      Email = request.Email,
      Address = request.Address,
      MaxBookLimit = request.MaxBookLimit
    };

    var generatedId = await _dbAccess.SaveData<Member, dynamic>
    (
      @"
        INSERT INTO member (name, email, address, max_book_limit, created_at, updated_at)
        VALUES (@Name, @Email, @Address, @MaxBookLimit, @CreatedAt, @UpdatedAt)
      ",
      data
    );
    data.Id = generatedId;

    return data;
  }

  public async Task Delete(int id)
  {
    await _dbAccess.SaveData<Member, dynamic>("DELETE FROM member WHERE id = @Id", new { Id = id });
  }

  public async Task<Member> Get(int id)
  {
    var result = await _dbAccess.GetData<Member, dynamic>
    (
      @"
        SELECT
          id AS Id,
          name AS Name,
          email AS Email,
          address AS Address,
          max_book_limit AS MaxBookLimit,
          created_at AS CreatedAt,
          updated_at AS UpdatedAt
        FROM member WHERE id = @Id
      ",
      new { Id = id }
    );

    return result.FirstOrDefault()!;
  }

  public async Task<IList<Member>> GetAll()
  {
    var result = await _dbAccess.GetData<Member, dynamic>
    (
      @"
        SELECT
          id AS Id,
          name AS Name,
          email AS Email,
          address AS Address,
          max_book_limit AS MaxBookLimit,
          created_at AS CreatedAt,
          updated_at AS UpdatedAt
        FROM member
      ", new {}
    );
    return result.ToList();
  }

  public async Task<Member> Update(int id, Member request)
  {
    Member data = new()
    {
      Id = id,
      Name = request.Name,
      Email = request.Email,
      Address = request.Address,
      MaxBookLimit = request.MaxBookLimit,
      CreatedAt = request.CreatedAt
    };

    await _dbAccess.SaveData<Member, dynamic>
    (
      @"
        UPDATE member m SET
          name = @Name,
          email = @Email,
          address = @Address,
          max_book_limit = @MaxBookLimit,
          created_at = @CreatedAt,
          updated_at = @UpdatedAt
        WHERE m.id = @Id
      ", 
      data
    );
    return data;
  }
}