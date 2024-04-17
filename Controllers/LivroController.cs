using Biblioteca.Context;
using Biblioteca.Entities;
using Biblioteca.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers;
[ApiController]
[Route("[controller]")]
public class LivroController : ControllerBase
{
    private readonly ILivroRepository _repository;

    public LivroController(ILivroRepository repository)
    {
        _repository = repository;
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<Livro>>> ObterTodos()
    {
        var livro = await _repository.GetAll();

        return Ok(livro.Select(livro => new
        {
            livro.Id,
            livro.Nome,
            livro.NumeroDePaginas,
            livro.Disponivel,
            livro.DataPublicacao,
            autor = new
            {
                livro.Autor.Id,
                livro.Autor.NomeCompleto,
                livro.Autor.Idade,
                livro.Autor.Nacionalidade
            }
        }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<Livro>>> ObterPorId(int id)
    {
        var livro = await _repository.GetById(id);

        if (livro == null)
            return BadRequest($"Livro de ID {id} não foi encontrado.");

        return Ok(new
        {
            livro.Id,
            livro.Nome,
            livro.NumeroDePaginas,
            livro.Disponivel,
            livro.DataPublicacao,
            autor = new
            {
                livro.Autor.Id,
                livro.Autor.NomeCompleto,
                livro.Autor.Idade,
                livro.Autor.Nacionalidade
            }
        });
    }
    [HttpPost("novo-livro")]
    public async Task<ActionResult<Livro>> NovoLivro([FromBody] Livro model)
    {
        try
        {
            if (model == null)
                return BadRequest("Dados inseridos inválidos.");

            var novoLivro = await _repository.Add(model);

            if (novoLivro == null)
                return BadRequest("Novo livro inválido.");

            if (string.IsNullOrEmpty(novoLivro.Nome))
                return BadRequest("Insira um nome válido.");

            if (novoLivro.NumeroDePaginas == 0)
                return BadRequest("O número de páginas tem que ser maior do que 0.");

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

            var livro = await _repository.GetById(id);

            if (livro == null)
                return BadRequest($"Livro de ID {id} não existe.");

            livro.Nome = model.Nome;
            livro.NumeroDePaginas = model.NumeroDePaginas;
            livro.Disponivel = model.Disponivel;
            livro.DataPublicacao = model.DataPublicacao;
            livro.AutorId = model.AutorId;

            await _repository.Update(livro);

            return Ok(livro);
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
            var livro = await _repository.GetById(id);

            if (livro == null)
                return BadRequest($"Livro de ID {id} não existe.");

            await _repository.Delete(livro);

            return NoContent();
        }
        catch (Exception)
        {

            throw;
        }
    }
}
