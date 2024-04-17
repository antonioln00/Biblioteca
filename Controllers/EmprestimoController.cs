using Biblioteca.Entities;
using Biblioteca.Interfaces;
using Biblioteca.Services;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers;
[ApiController]
[Route("[controller]")]
public class EmprestimoController : ControllerBase
{
    private readonly IEmprestimoRepository _repository;
    private readonly EmprestimoService _emprestimoService;

    public EmprestimoController(IEmprestimoRepository repository, EmprestimoService emprestimoService)
    {
        _repository = repository;
        _emprestimoService = emprestimoService;
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<Emprestimo>>> ObterTodos()
    {
        var emprestimos = await _repository.GetAll();
        return Ok(emprestimos.Select(emprestimo => new
        {
            emprestimo.Id,
            emprestimo.NomeCompleto,
            emprestimo.RG,
            emprestimo.Endereco,
            emprestimo.Complemento,
            emprestimo.Numero,
            emprestimo.DataNascimento,
            emprestimo.DataDevolucao,
            livro = emprestimo.Livro != null ? new
            {
                emprestimo.Livro.Id,
                emprestimo.Livro.Nome,
                emprestimo.Livro.NumeroDePaginas,
                emprestimo.Livro.Disponivel,
                emprestimo.Livro.DataPublicacao,
                autor = emprestimo.Livro.Autor != null ? new
                {
                    emprestimo.Livro.Autor.Id,
                    emprestimo.Livro.Autor.NomeCompleto,
                    emprestimo.Livro.Autor.Idade,
                    emprestimo.Livro.Autor.Nacionalidade
                } : null
            } : null
        }));
    }

    [HttpPost("novo-emprestimo")]
    public async Task<ActionResult<Emprestimo>> NovoEmprestimo([FromBody] Emprestimo model)
    {
        try
        {
            if (model == null)
                return BadRequest("Dados inseridos inválidos.");

            var novoEmprestimo = await _repository.Add(model);

            if (novoEmprestimo == null)
                return BadRequest("Novo empréstimo inválido.");

            if (string.IsNullOrEmpty(novoEmprestimo.NomeCompleto))
                return BadRequest("Insira um nome válido.");

            if (string.IsNullOrEmpty(novoEmprestimo.RG))
                return BadRequest("Insira um RG válido.");

            if (string.IsNullOrEmpty(novoEmprestimo.Endereco))
                return BadRequest("Insira um Endereco válido.");

            if (string.IsNullOrEmpty(novoEmprestimo.Numero))
                return BadRequest("Insira um Numero válido.");

            if (!await _emprestimoService.VerificarDisponibilidadeLivro(model.LivroId))
            {
                // var livro = await _context.Livros.FindAsync(model.LivroId);

                // livro.Disponivel = false;
                // _context.Livros.Update(livro);
            }
            else
            {
                return BadRequest($"Livro de ID {model.LivroId} não está disponível para empréstimo.");
            }

            return Ok(model);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPut("atualizar-emprestimo/{id:int}")]
    public async Task<ActionResult<Emprestimo>> AtualizarEmprestimo(int id, Emprestimo model)
    {
        try
        {
            if (model == null)
                return BadRequest("Dados inseridos inválidos.");

            var emprestimo = await _repository.GetById(id);

            if (emprestimo == null)
                return BadRequest($"Empréstimo de ID {id} não existe.");

            emprestimo.NomeCompleto = model.NomeCompleto;
            emprestimo.RG = model.RG;
            emprestimo.Endereco = model.Endereco;
            emprestimo.Complemento = model.Complemento;
            emprestimo.Numero = model.Numero;
            emprestimo.DataNascimento = model.DataNascimento;
            emprestimo.LivroId = model.LivroId;

            await _repository.Update(emprestimo);

            return Ok(emprestimo);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpDelete("deletar-emprestimo/{id}")]
    public async Task<ActionResult> DeletarEmprestimo(int id)
    {
        try
        {
            var emprestimo = await _repository.GetById(id);

            if (emprestimo == null)
                return BadRequest($"Emprestimo de ID {id} não existe.");

            // var livro = await _context.Livros.FindAsync(emprestimo.LivroId);

            // livro.Disponivel = true;
            // _context.Livros.Update(livro);

            await _repository.Delete(emprestimo);
            return NoContent();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
