using GerenciadorFinanceiro.Domain.Entities;
using GerenciadorFinanceiro.Domain.Models;

namespace GerenciadorFinanceiro.Domain.Interfaces
{
    public  interface ICategoriaRepository
    {
        Task AdicionarAsync(Categoria categoria);
        Task<IEnumerable<Categoria>> ObterTodosAsync();
        Task<Categoria?> ObterPorIdAsync(Guid id);
        Task RemoveAsync(Categoria categoria);
        Task EditAsync(Categoria categoria);
    }
}
