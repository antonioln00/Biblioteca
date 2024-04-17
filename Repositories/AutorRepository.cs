using Biblioteca.Context;
using Biblioteca.Entities;
using Biblioteca.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositories;
public class AutorRepository : Repository<Autor>, IAutorRepository
{
    public AutorRepository(ApplicationDbContext context) : base(context) { }

    public async override Task<IEnumerable<Autor>> GetAll() =>
        await _context.Autores.Include(e => e.Livros).ToListAsync();

    public async override Task<Autor> GetById(int id) =>
        await _context.Autores.Include(e => e.Livros).FirstOrDefaultAsync(e => e.Id == id);
}