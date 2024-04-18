using Biblioteca.Context;
using Biblioteca.Entities;
using Biblioteca.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositories;
public class LivroRepository : Repository<Livro>, ILivroRepository
{
    public LivroRepository(ApplicationDbContext context) : base(context) { }

    public override async Task<IEnumerable<Livro>> GetAll() =>
        await _context.Livros.Include(e => e.Autor).ToListAsync();

    public override async Task<Livro> GetById(int id) =>
        await _context.Livros.Include(e => e.Autor).FirstOrDefaultAsync(e => e.Id == id);
}