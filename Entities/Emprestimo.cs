namespace Biblioteca.Entities;
public class Emprestimo
{
    public int Id { get; set; }
    public string NomeCompleto { get; set; }
    public string RG { get; set; }
    public string Endereco { get; set; }
    public string? Complemento { get; set; }
    public string Numero { get; set; }
    public DateTime DataNascimento { get; set; }
    public DateTime DataDevolucao { get; set; }
    public int LivroId { get; set; }
    public virtual Livro? Livro { get; set; }
    public Emprestimo()
    {
        DataDevolucao = DateTime.Now.AddDays(14);
    }
}
