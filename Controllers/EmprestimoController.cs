using Biblioteca.Context;
using Biblioteca.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers;
[ApiController]
[Route("[controller]")]
public class EmprestimoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EmprestimoController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<Emprestimo>>> ObterTodos() =>
        Ok(await _context.Emprestimos.Select(emprestimo => new {
            emprestimo.Id,
            emprestimo.NomeCompleto,
            emprestimo.RG,
            emprestimo.Endereco,
            emprestimo.Complemento,
            emprestimo.Numero,
            emprestimo.DataNascimento,
            emprestimo.DataDevolucao,
            livro = new {
                emprestimo.Livro.Id,
                emprestimo.Livro.Nome,
                emprestimo.Livro.NumeroDePaginas,
                emprestimo.Livro.Disponivel,
                emprestimo.Livro.DataPublicacao,
                autor = new {
                   emprestimo.Livro.Autor.Id,
                   emprestimo.Livro.Autor.NomeCompleto,
                   emprestimo.Livro.Autor.Idade,
                   emprestimo.Livro.Autor.Nacionalidade 
                }
            }
        }).ToListAsync());

    [HttpPost("novo-emprestimo")]
    public async Task<ActionResult<Emprestimo>> NovoEmprestimo([FromBody] Emprestimo model)
    {
        try
        {
            if (model == null)
                return BadRequest("Dados inseridos inválidos.");

            var novoEmprestimo = new Emprestimo
            {
                NomeCompleto = model.NomeCompleto,
                RG = model.RG,
                Endereco = model.Endereco,
                Complemento = model.Complemento,
                Numero = model.Numero,
                DataNascimento = model.DataNascimento,
                LivroId = model.LivroId
            };

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

            if (!await VerificarDisponibilidadeLivro(model.LivroId))
            {
                var livro = await _context.Livros.FindAsync(model.LivroId);

                livro.Disponivel = false;
                _context.Livros.Update(livro);
            }
            else
            {
                return BadRequest($"Livro de ID {model.LivroId} não está disponível para empréstimo.");
            }


            _context.Emprestimos.Add(novoEmprestimo);
            await _context.SaveChangesAsync();

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

            var emprestimo = await _context.Emprestimos.FindAsync(id);

            if (emprestimo == null)
                return BadRequest($"Empréstimo de ID {id} não existe.");

            emprestimo.NomeCompleto = model.NomeCompleto;
            emprestimo.RG = model.RG;
            emprestimo.Endereco = model.Endereco;
            emprestimo.Complemento = model.Complemento;
            emprestimo.Numero = model.Numero;
            emprestimo.DataNascimento = model.DataNascimento;
            emprestimo.LivroId = model.LivroId;

            _context.Emprestimos.Update(emprestimo);
            await _context.SaveChangesAsync();

            return Ok(model);
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
            var emprestimo = await _context.Emprestimos.FindAsync(id);

            if (emprestimo == null)
                return BadRequest($"Emprestimo de ID {id} não existe.");

            var livro = await _context.Livros.FindAsync(emprestimo.LivroId);

            livro.Disponivel = true;
            _context.Livros.Update(livro);

            _context.Emprestimos.Remove(emprestimo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task<bool> VerificarDisponibilidadeLivro(int id)
    {
        try
        {
            var livros = await _context.Livros.FirstOrDefaultAsync(e => e.Id == id);
            if (livros != null && livros.Disponivel == false)
                return true;

            return false;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
