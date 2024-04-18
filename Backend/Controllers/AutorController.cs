using Biblioteca.Entities;
using Biblioteca.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers;
[ApiController]
[Route("[controller]")]
public class AutorController : ControllerBase
{
    private readonly IAutorRepository _repository;

    public AutorController(IAutorRepository repository)
    {
        _repository = repository;
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<Autor>>> ObterTodos()
    {
        var getAll = await _repository.GetAll();
        return Ok(getAll.Select(autor => new
        {
            autor.Id,
            autor.NomeCompleto,
            autor.Idade,
            autor.Nacionalidade,
            Livros = autor.Livros.Select(livro => new
            {
                livro.Id,
                livro.Nome,
                livro.NumeroDePaginas,
                livro.Disponivel,
                livro.DataPublicacao
            })
        }));
    }

    [HttpPost("novo-autor")]
    public async Task<ActionResult<Autor>> NovoAutor([FromBody] Autor model)
    {
        try
        {
            if (model == null)
                return BadRequest("Dados inseridos inválidos.");

            var novoAutor = await _repository.Add(model);

            if (string.IsNullOrEmpty(novoAutor.NomeCompleto))
                return BadRequest("Insira um nome válido.");

            if (novoAutor.Idade == 0)
                return BadRequest("A idade tem que ser maior do que 0.");

            if (string.IsNullOrEmpty(novoAutor.Nacionalidade))
                return BadRequest("Insira uma nacionalidade válida.");

            if (novoAutor == null)
                return BadRequest("Novo autor inválido.");

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

            var autor = await _repository.GetById(id);

            if (autor == null)
                return BadRequest($"Autor de ID {id} não existe.");

            autor.NomeCompleto = model.NomeCompleto;
            autor.Idade = model.Idade;
            autor.Nacionalidade = model.Nacionalidade;

            await _repository.Update(autor);
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
            var autor = await _repository.GetById(id);

            if (autor == null)
                return BadRequest($"Autor de ID {id} não existe.");

            if (autor.Livros.Any())
                return BadRequest("Não foi possível excluir esse autor porque há livros atrelados a ele.");

            await _repository.Delete(autor);
            return NoContent();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
