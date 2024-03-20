using Biblioteca.Context;
using Biblioteca.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers;
[ApiController]
[Route("[controller]")]
public class AutorController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AutorController(ApplicationDbContext context)
    {
        _context = context;   
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<Autor>>> ObterTodos() => 
        Ok(await _context.Autores.Select(autor => new {
        autor.Id,
        autor.NomeCompleto,
        autor.Idade,
        autor.Nacionalidade,
        Livros = autor.Livros.Select(livro => new {
            livro.Id,
            livro.Nome,
            livro.NumeroDePaginas,
            livro.Disponivel,
            livro.DataPublicacao
        })
    }).ToListAsync());

    [HttpPost("novo-autor")]
    public async Task<ActionResult<Autor>> NovoAutor([FromBody] Autor model)
    {
        try
        {
            if (model == null)
                return BadRequest("Dados inseridos inválidos.");

            var novoAutor = new Autor {
                NomeCompleto = model.NomeCompleto,
                Idade = model.Idade,
                Nacionalidade = model.Nacionalidade
            };

            if (string.IsNullOrEmpty(novoAutor.NomeCompleto))
                return BadRequest("Insira um nome válido.");

            if (novoAutor.Idade == 0)
                return BadRequest("A idade tem que ser maior do que 0.");

            if (string.IsNullOrEmpty(novoAutor.Nacionalidade))
                return BadRequest("Insira uma nacionalidade válida.");

            if (novoAutor == null)
                return BadRequest("Novo autor inválido.");

            _context.Autores.Add(novoAutor);
            await _context.SaveChangesAsync();

            return Ok(model);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPut("atualizar-autor/{id:int}")]
    public async Task<ActionResult<Autor>> AtualizarAutor(int id, Autor model)
    {
        try
        {
            if (model == null)
                return BadRequest("Dados inseridos inválidos.");

            var autor = await _context.Autores.FindAsync(id);

            if (autor == null)
                return BadRequest($"Autor de ID {id} não existe.");
            
            autor.NomeCompleto = model.NomeCompleto;
            autor.Idade = model.Idade;
            autor.Nacionalidade = model.Nacionalidade;

            _context.Autores.Update(autor);
            await _context.SaveChangesAsync();

            return Ok(model);
        }
        catch (Exception)
        {
            throw;
        }
    }
    [HttpDelete("deletar-autor/{id}")]
    public async Task<ActionResult> DeletarAutor(int id)
    {
        try
        {
            var autor = await _context.Autores.Include(e=> e.Livros).FirstOrDefaultAsync(e => e.Id == id);

            if (autor == null)
                return BadRequest($"Autor de ID {id} não existe.");

            if (autor.Livros.Any())
                return BadRequest("Não foi possível excluir esse autor porque há livros atrelados a ele.");

            _context.Autores.Remove(autor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception)
        {
            
            throw;
        }
    }
}
