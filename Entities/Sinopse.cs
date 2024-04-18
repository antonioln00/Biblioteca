namespace Biblioteca.Entities;
public class Sinopse : Base<int>
{
    public string Genero { get; set; }
    public string Resenha { get; set; }
    public int LivroId { get; set; }
    public virtual Livro? Livro { get; set; }
}
