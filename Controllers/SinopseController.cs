using Biblioteca.Context;
using Biblioteca.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers;
[ApiController]
[Route("[controller]")]
public class SinopseController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SinopseController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<Sinopse>>> ObterTodos() => 
        Ok (await _context.Sinopses.Select(sinopse => new {
            sinopse.Id,
            sinopse.Genero,
            sinopse.Resenha,
            livro = new {
                sinopse.Livro.Id,
                sinopse.Livro.Nome,
                sinopse.Livro.NumeroDePaginas,
                sinopse.Livro.Disponivel,
                sinopse.Livro.DataPublicacao,
                autor = new {
                   sinopse.Livro.Autor.Id,
                   sinopse.Livro.Autor.NomeCompleto,
                   sinopse.Livro.Autor.Idade,
                   sinopse.Livro.Autor.Nacionalidade 
                }
            }
        }).ToListAsync());

    [HttpPost("nova-sinopse")]
    public async Task<ActionResult<Sinopse>> NovaSinopse([FromBody] Sinopse model)
    {
        try
        {
            if (model == null)
                return BadRequest("Dados inseridos inválidos.");

            var novaSinopse = new Sinopse {
                Genero = model.Genero,
                Resenha = model.Resenha,
                LivroId = model.LivroId
            };

            if (novaSinopse == null)
                return BadRequest("Nova sinopse inválida.");

            if (string.IsNullOrEmpty(novaSinopse.Genero))
                return BadRequest("Insira um gênero válido.");

            if (string.IsNullOrEmpty(novaSinopse.Resenha))
                return BadRequest("Insira uma resenha válido.");

            _context.Sinopses.Add(novaSinopse);
            await _context.SaveChangesAsync();

            return Ok(model);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPut("atualizar-sinopse/{id:int}")]
    public async Task<ActionResult<Sinopse>> AtualizarSinopse(int id, Sinopse model)
    {
        try
        {
            if (model == null)
                return BadRequest("Dados inseridos inválidos.");

            var sinopse = await _context.Sinopses.FindAsync(id);

            if (sinopse == null)
                return BadRequest($"Sinopse de ID {id} não existe.");
            
            sinopse.Genero = model.Genero;
            sinopse.Resenha = model.Resenha;
            sinopse.LivroId = model.LivroId;

            _context.Sinopses.Update(sinopse);
            await _context.SaveChangesAsync();

            return Ok(model);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpDelete("deletar-sinopse/{id}")]
    public async Task<ActionResult> DeletarSinopse(int id)
    {
        try
        {
            var sinopse = await _context.Sinopses.FindAsync(id);

            if (sinopse == null)
                return BadRequest($"Sinopse de ID {id} não existe.");
            
            _context.Sinopses.Remove(sinopse);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
