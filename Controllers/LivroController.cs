using Biblioteca.Context;
using Biblioteca.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers;
[ApiController]
[Route("[controller]")]
public class LivroController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public LivroController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<Livro>>> ObterTodos() =>
        Ok(await _context.Livros.Select(livro => new {
            livro.Id,
            livro.Nome,
            livro.NumeroDePaginas,
            livro.Disponivel,
            livro.DataPublicacao,
            autor = new {
                livro.Autor.Id,
                livro.Autor.NomeCompleto,
                livro.Autor.Idade,
                livro.Autor.Nacionalidade
            }
        }).ToListAsync());

    [HttpPost("novo-livro")]
    public async Task<ActionResult<Livro>> NovoLivro([FromBody]Livro model)
    {
        try
        {
            if (model == null)
                return BadRequest("Dados inseridos inválidos.");

            var novoLivro = new Livro {
                Nome = model.Nome,
                NumeroDePaginas = model.NumeroDePaginas,
                Disponivel = model.Disponivel,
                DataPublicacao = model.DataPublicacao,
                AutorId = model.AutorId
            };

            if (novoLivro == null)
                return BadRequest("Novo livro inválido.");

            if (string.IsNullOrEmpty(novoLivro.Nome))
                return BadRequest("Insira um nome válido.");

            if (novoLivro.NumeroDePaginas == 0)
                return BadRequest("O número de páginas tem que ser maior do que 0.");
            
            _context.Livros.Add(novoLivro);
            await _context.SaveChangesAsync();

            return Ok(model);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPut("atualizar-livro/{id:int}")]
    public async Task<ActionResult<Livro>> AtualizarLivro(int id, Livro model)
    {
        try
        {
            if (model == null)
                return BadRequest("Dados inseridos inválidos.");

            var livro = await _context.Livros.FindAsync(id);

            if (livro == null)
                return BadRequest($"Livro de ID {id} não existe.");

            livro.Nome = model.Nome;
            livro.NumeroDePaginas = model.NumeroDePaginas;
            livro.Disponivel = model.Disponivel;
            livro.DataPublicacao = model.DataPublicacao;
            livro.AutorId = model.AutorId;

            _context.Livros.Update(livro);
            await _context.SaveChangesAsync();

            return Ok(model);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpDelete("deletar-livro/{id}")]
    public async Task<ActionResult> DeletarLivro(int id)
    {
        try
        {
            var livro = await _context.Livros.FirstOrDefaultAsync(e => e.Id == id);
            var emprestimo = await _context.Emprestimos.Include(e => e.Livro).FirstOrDefaultAsync(e=> e.LivroId == id);

            if (livro == null)
                return BadRequest($"Livro de ID {id} não existe.");

            if (livro.Disponivel == false)
                return BadRequest($"Não foi possível excluir esse livro porque ele está emprestado para {emprestimo.Id}: {emprestimo.NomeCompleto}.");

            _context.Livros.Remove(livro);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception)
        {
            
            throw;
        }
    }
}
