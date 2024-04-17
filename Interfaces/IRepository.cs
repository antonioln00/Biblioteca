namespace Biblioteca.Interfaces;
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<T> GetById(int id);
    Task<T> Add(T model);
    Task Update(T model);
    Task Delete(T model);
}
