using Biblioteca.Context;
using Biblioteca.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositories;
public class Repository<T> : IRepository<T> where T : class
{
    public readonly ApplicationDbContext _context;
    
    public Repository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<T> Add(T model)
    {
        _context.Set<T>().Add(model);
        await _context.SaveChangesAsync();
        return model;
    }

    public async Task Delete(T model)
    {
        _context.Set<T>().Remove(model);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAll()
     => await _context.Set<T>().ToListAsync();

    public virtual async Task<T> GetById(int id)
     => await _context.Set<T>().FindAsync(id);

    public async Task Update(T model)
    {
        _context.Set<T>().Update(model);
        await _context.SaveChangesAsync();
    }
}