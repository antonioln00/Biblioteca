using Biblioteca.Context;
using Biblioteca.Entities;
using Biblioteca.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositories;
public class SinopseRepository : Repository<Sinopse>, ISinopseRepository
{
    public SinopseRepository(ApplicationDbContext context) : base(context) { }

    public override async Task<IEnumerable<Sinopse>> GetAll() =>
        await _context.Sinopses.Include(e => e.Livro).Include(e => e.Livro.Autor).ToListAsync();

    public override async Task<Sinopse> GetById(int id) =>
        await _context.Sinopses.Include(e => e.Livro).Include(e => e.Livro.Autor).FirstOrDefaultAsync(e => e.Id == id);
}