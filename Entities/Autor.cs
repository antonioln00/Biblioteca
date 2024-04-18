namespace Biblioteca.Entities;
public class Autor : Base<int>
{
    public string NomeCompleto { get; set; }
    public int Idade { get; set; }
    public string Nacionalidade { get; set; }
    public virtual IEnumerable<Livro>? Livros { get; set; }
}
