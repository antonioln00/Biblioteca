namespace Biblioteca.Entities;
public class Livro
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int NumeroDePaginas { get; set; }
    public bool Disponivel { get; set; }
    public DateTime DataPublicacao { get; set; }
    public int AutorId { get; set; }
    public virtual Autor? Autor { get; set; }
}
