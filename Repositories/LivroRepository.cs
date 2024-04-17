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

    public override async Task Delete(Livro model)
    {
        var emprestimo = await _context.Emprestimos.Include(e => e.Livro).FirstOrDefaultAsync(e => e.LivroId == model.Id);

        if (model.Disponivel == false)
            new Exception($"Não foi possível excluir esse livro porque ele está emprestado para {emprestimo.Id}: {emprestimo.NomeCompleto}.");

    }
}