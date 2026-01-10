using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GerenciadorFinanceiro.Domain.Entities;
using GerenciadorFinanceiro.Domain.Interfaces;
using GerenciadorFinanceiro.Infrastructure.Context;
using GerenciadorFinanceiro.Domain.Models;

namespace GerenciadorFinanceiro.Infrastructure.Repositories
{
    public class LancamentoRepository : ILancamentoRepository
    {
        private readonly FinanceiroDbContext _context;

        public LancamentoRepository(FinanceiroDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Lancamento lancamento)
        {
            var maximoDespesasNoMes = await _context.Categorias
                .Where(c => c.Id == lancamento.CategoriaId)
                .Select(c => c.OrcamentoMensal)
                .FirstOrDefaultAsync();

            var totalDespesasNoMes = await _context.Lancamentos
                .Where(l => l.CategoriaId == lancamento.CategoriaId &&
                            l.Tipo == TipoLancamento.Despesa &&
                            l.DataLancamento.Month == lancamento.DataLancamento.Month &&
                            l.DataLancamento.Year == lancamento.DataLancamento.Year)
                .SumAsync(l => l.Valor);

            if (totalDespesasNoMes + lancamento.Valor > maximoDespesasNoMes)
                throw new Exception("O orçamento mensal para esta categoria foi excedido.");

            await _context.Lancamentos.AddAsync(lancamento);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Lancamento>> ObterTodosAsync()
        {
            return await _context.Lancamentos
                .Include(x => x.Categoria)
                .ToListAsync();
        }

        public async Task<Lancamento?> ObterPorIdAsync(Guid id)
        {
            return await _context.Lancamentos
                .Include(x => x.Categoria)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Lancamento>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim)
        {
            var datainicio = inicio.Date;
            var dataFim = fim.Date.AddDays(1).AddTicks(-1);
                
            return await _context.Lancamentos
                .Include(x => x.Categoria)
                .Where(x => x.DataLancamento >= datainicio && x.DataLancamento < dataFim)
                .ToListAsync();
        }

        public async Task RemoveAsync(Lancamento lancamento)
        {
            _context.Lancamentos.Remove(lancamento);
            await _context.SaveChangesAsync();
        }

        public Task EditAsync(Lancamento lancamento)
        {
            _context.Lancamentos.Update(lancamento);
            return _context.SaveChangesAsync();
        }

        public async Task<ResumoFinanceiro> ObterResumoAsync()
        {
            var receitas = await _context.Lancamentos
                 .Where(x => x.Tipo == Domain.Entities.TipoLancamento.Receita)
                 .SumAsync(x => x.Valor);

            var despesas = await _context.Lancamentos
                .Where(x => x.Tipo == Domain.Entities.TipoLancamento.Despesa)
                .SumAsync(x => x.Valor);

            var saldo = receitas - despesas;

            return new ResumoFinanceiro(receitas, despesas, saldo);
        }

        public async Task<IEnumerable<RelatorioCategoria>> ObterTotaisCategoriaAsync()
        {
            return await _context.Lancamentos
                .Include(x => x.Categoria)
                .GroupBy(x => x.Categoria.Nome)
                .Select(g => new RelatorioCategoria(g.Key, g.Sum(x => x.Valor)))
                .ToListAsync();
        }

        public async Task<IEnumerable<Categoria>> CategoriaExisteAsync(Guid categoriaId)
        {
            return await _context.Categorias
                .Where(x => x.Id == categoriaId)
                .ToListAsync();
        }

        public async Task<Lancamento?> ObterLancamentoPorTipo(TipoLancamento tipo)
        {
            return await _context.Lancamentos
                .FirstOrDefaultAsync(l => l.Tipo == tipo);
        }
    }
}