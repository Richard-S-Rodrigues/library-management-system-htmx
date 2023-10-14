namespace LibraryManagementSystemHtmx.Services;

public interface ICrudService<T> 
{
  Task<IEnumerable<T>> GetAll();
  Task<T> Get(int id);
  Task<T> Create(T request);
  Task<T> Update(int id, T request);
  Task Delete(int id);
}