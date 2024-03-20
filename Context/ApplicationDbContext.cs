using Biblioteca.Entities;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Context;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base (options) { }

    public DbSet<Autor> Autores { get; set; }
    public DbSet<Emprestimo> Emprestimos { get; set; }
    public DbSet<Livro> Livros { get; set; }
    public DbSet<Sinopse> Sinopses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Livro>() // na tabela livro
            .HasOne(e => e.Autor) // tem um autor
            .WithMany(e => e.Livros) // para muitos livros
            .HasForeignKey(e => e.AutorId) // referenciados pela chave estrangeira autor id
            .IsRequired(); // e não pode existir um livro sem ter um autor

        modelBuilder
            .Entity<Emprestimo>() // na tabela emprestimo
            .HasOne(e => e.Livro) // tem um livro
            .WithOne() // para um emprestimo
            .HasForeignKey<Emprestimo>(e => e.LivroId) // referenciado pela chave estrangeira livro id
            .IsRequired(); // e não pode exisitr um emprestimo sem ter um livro

        modelBuilder
            .Entity<Sinopse>()
            .HasOne(e => e.Livro)
            .WithOne()
            .HasForeignKey<Sinopse>(e => e.LivroId)
            .IsRequired();

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer("DefaultConnection");
    
        base.OnConfiguring(optionsBuilder);
    }
}
