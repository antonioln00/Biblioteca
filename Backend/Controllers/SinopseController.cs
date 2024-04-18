using Biblioteca.Entities;
using Biblioteca.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers;
[ApiController]
[Route("[controller]")]
public class SinopseController : ControllerBase
{
    private readonly ISinopseRepository _sinopseRepository;

    public SinopseController(ISinopseRepository sinopseRepository)
    {
        _sinopseRepository = sinopseRepository;
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<Sinopse>>> ObterTodos()
    {
        var sinopse = await _sinopseRepository.GetAll();
        
        return Ok(sinopse.Select(sinopse => new
        {
            sinopse.Id,
            sinopse.Genero,
            sinopse.Resenha,
            livro = new
            {
                sinopse.Livro.Id,
                sinopse.Livro.Nome,
                sinopse.Livro.NumeroDePaginas,
                sinopse.Livro.Disponivel,
                sinopse.Livro.DataPublicacao,
                autor = new
                {
                    sinopse.Livro.Autor.Id,
                    sinopse.Livro.Autor.NomeCompleto,
                    sinopse.Livro.Autor.Idade,
                    sinopse.Livro.Autor.Nacionalidade
                }
            }
        }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<Sinopse>>> ObterPorId(int id)
    {
        var sinopse = await _sinopseRepository.GetById(id);
        return Ok(new
        {
            sinopse.Id,
            sinopse.Genero,
            sinopse.Resenha,
            livro = new
            {
                sinopse.Livro.Id,
                sinopse.Livro.Nome,
                sinopse.Livro.NumeroDePaginas,
                sinopse.Livro.Disponivel,
                sinopse.Livro.DataPublicacao,
                autor = new
                {
                    sinopse.Livro.Autor.Id,
                    sinopse.Livro.Autor.NomeCompleto,
                    sinopse.Livro.Autor.Idade,
                    sinopse.Livro.Autor.Nacionalidade
                }
            }
        });
    }

    [HttpPost("nova-sinopse")]
    public async Task<ActionResult<Sinopse>> NovaSinopse([FromBody] Sinopse model)
    {
        try
        {
            if (model == null)
                return BadRequest("Dados inseridos inválidos.");

            var novaSinopse = await _sinopseRepository.Add(model);

            if (novaSinopse == null)
                return BadRequest("Nova sinopse inválida.");

            if (string.IsNullOrEmpty(novaSinopse.Genero))
                return BadRequest("Insira um gênero válido.");

            if (string.IsNullOrEmpty(novaSinopse.Resenha))
                return BadRequest("Insira uma resenha válido.");

            return Ok(novaSinopse);
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

            var sinopse = await _sinopseRepository.GetById(id);

            if (sinopse == null)
                return BadRequest($"Sinopse de ID {id} não existe.");

            sinopse.Genero = model.Genero;
            sinopse.Resenha = model.Resenha;
            sinopse.LivroId = model.LivroId;

            await _sinopseRepository.Update(sinopse);

            return Ok(sinopse);
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
            var sinopse = await _sinopseRepository.GetById(id);

            if (sinopse == null)
                return BadRequest($"Sinopse de ID {id} não existe.");

            await _sinopseRepository.Delete(sinopse);

            return NoContent();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
