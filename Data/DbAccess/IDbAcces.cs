namespace LibraryManagementSystemHtmx.Data.DbAccess;

public interface IDbAccess 
{
  Task<IEnumerable<T>> GetData<T, U>(
    string storedProcedure,
    U parameters,
    string connectionId = "DefaultConnection"
  );

  Task<int?> SaveData<T, U>(
    string storedProcedure,
    U parameters,
    string connectionId = "DefaultConnection"
  );
}