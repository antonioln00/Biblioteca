using Biblioteca.Context;
using Biblioteca.Entities;
using Biblioteca.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositories;
public class EmprestimoRepository : Repository<Emprestimo>, IEmprestimoRepository
{
    public EmprestimoRepository(ApplicationDbContext context) : base(context) { }

    public async override Task<IEnumerable<Emprestimo>> GetAll() =>
        await _context.Emprestimos
            .Include(e => e.Livro)
            .Include(e => e.Livro.Autor)
            .ToListAsync();

    public async override Task<Emprestimo> GetById(int id) =>
        await _context.Emprestimos
            .Include(e => e.Livro)
            .Include(e => e.Livro.Autor)
            .FirstOrDefaultAsync(e => e.Id == id);
}
