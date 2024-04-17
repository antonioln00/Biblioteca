using Biblioteca.Context;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Services;
public class EmprestimoService
{
    private readonly ApplicationDbContext _context;

    public EmprestimoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> VerificarDisponibilidadeLivro(int id)
    {
        try
        {
            var livro = _context.Livros.FirstOrDefaultAsync(e => e.Id == id).GetAwaiter().GetResult();
            if (livro.Id != null && livro.Disponivel == false)
                return true;

            return false;
        }
        catch (Exception)
        {
            throw;
        }
    }
}