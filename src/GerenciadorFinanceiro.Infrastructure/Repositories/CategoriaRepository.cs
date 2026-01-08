using GerenciadorFinanceiro.Domain.Entities;
using GerenciadorFinanceiro.Domain.Interfaces;
using GerenciadorFinanceiro.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFinanceiro.Infrastructure.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly FinanceiroDbContext _context;

        public CategoriaRepository(FinanceiroDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Categoria>> ObterTodosAsync()
        {
            return await _context.Categorias.ToListAsync();
        }

        public async Task<Categoria?> ObterPorIdAsync(Guid id)
        {
            return await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AdicionarAsync(Categoria categoria)
        {
            await _context.Categorias.AddAsync(categoria);
        }

        public Task EditAsync(Categoria categoria)
        {
            _context.Categorias.Update(categoria);
            return _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Categoria categoria)
        {
            var ExisteLancamento = _context.Lancamentos.Any(l => l.CategoriaId == categoria.Id);
            if (ExisteLancamento)
            {
                throw new Exception("Não é possível excluir a categoria pois existem lançamentos associados a ela.");
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
        }
    }
}
