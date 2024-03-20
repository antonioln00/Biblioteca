namespace Biblioteca.Entities;
public class Autor
{
    public int Id { get; set; }
    public string NomeCompleto { get; set; }
    public int Idade { get; set; }
    public string Nacionalidade { get; set; }
    public virtual IEnumerable<Livro>? Livros { get; set; }
}
