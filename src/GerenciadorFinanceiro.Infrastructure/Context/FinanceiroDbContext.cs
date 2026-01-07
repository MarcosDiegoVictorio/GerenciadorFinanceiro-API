using Microsoft.EntityFrameworkCore;
using GerenciadorFinanceiro.Domain.Entities;

namespace GerenciadorFinanceiro.Infrastructure.Context
{
    public class FinanceiroDbContext : DbContext
    {
        // O construtor recebe as opções (como a string de conexão) e passa para a classe base
        public FinanceiroDbContext(DbContextOptions<FinanceiroDbContext> options) : base(options)
        {
        }

        // Define a tabela de Lançamentos
        public DbSet<Lancamento> Lancamentos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Garante que o Id é a chave primária
            modelBuilder.Entity<Lancamento>().HasKey(x => x.Id);
            modelBuilder.Entity<Categoria>().HasKey(x => x.Id);

            modelBuilder.Entity<Lancamento>()
                .HasOne(l => l.Categoria)
                .WithMany()
                .HasForeignKey(l => l.CategoriaId);
        }
    }
}