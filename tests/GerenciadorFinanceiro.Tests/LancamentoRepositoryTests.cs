using GerenciadorFinanceiro.Domain.Entities;
using GerenciadorFinanceiro.Infrastructure.Context;
using GerenciadorFinanceiro.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorFinanceiro.Tests
{
    public class LancamentoRepositoryTests
    {
        [Fact] // Essa tag diz: "Isso é um teste"
        public async Task Deve_Calcular_Saldo_Corretamente()
        {
            // --- ARRANGE (Preparação do Cenário) ---
            // 1. Criamos um banco de dados falso na memória
            var options = new DbContextOptionsBuilder<FinanceiroDbContext>()
                .UseInMemoryDatabase(databaseName: "BancoTesteSaldo")
                .Options;

            // 2. Criamos o contexto e populamos com dados fictícios
            using (var context = new FinanceiroDbContext(options))
            {
                // Precisamos de uma categoria fake para não dar erro
                var catAlimentacao = new Categoria("Alimentacao",1500);
                var catSalario = new Categoria("Salario",7527);

                context.Categorias.AddRange(catAlimentacao, catSalario);
                await context.SaveChangesAsync();

                // Adiciona: Receita de 1000
                context.Lancamentos.Add(new Lancamento("Salario", 1000, TipoLancamento.Receita, catSalario.Id));

                // Adiciona: Despesa de 200
                context.Lancamentos.Add(new Lancamento("Pizza", 200, TipoLancamento.Despesa, catAlimentacao.Id));

                // Adiciona: Despesa de 100
                context.Lancamentos.Add(new Lancamento("Gasolina", 100, TipoLancamento.Despesa, catAlimentacao.Id));

                await context.SaveChangesAsync();
            }

            // --- ACT (Ação) ---
            // Agora instanciamos o Repositório usando esse banco falso e chamamos o método
            using (var context = new FinanceiroDbContext(options))
            {
                var repository = new LancamentoRepository(context);
                var resultado = await repository.ObterResumoAsync();

                // --- ASSERT (Verificação) ---
                // Esperamos: Receita 1000, Despesas 300 (200+100), Saldo 700.

                Assert.Equal(1000, resultado.TotalReceitas); // Verifica Receitas
                Assert.Equal(300, resultado.TotalDespesas);  // Verifica Despesas
                Assert.Equal(700, resultado.Saldo);          // Verifica se 1000 - 300 = 700
            }
        }
    }
}