using GerenciadorFinanceiro.Domain.Entities;
using GerenciadorFinanceiro.Domain.Models;

namespace GerenciadorFinanceiro.Domain.Interfaces
{
    public interface ILancamentoRepository
    {
        /// <summary>
        /// Add a new Lancamento entry.
        /// </summary>
        /// <param name="lancamento"></param>
        /// <returns></returns>
        Task AdicionarAsync(Lancamento lancamento);

        /// <summary>
        /// Get all Lancamento entries.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Lancamento>> ObterTodosAsync();

        /// <summary>
        /// get Lancamento entries by period.
        /// </summary>
        /// <param name="inicio"></param>
        /// <param name="fim"></param>
        /// <returns></returns>
        Task<IEnumerable<Lancamento>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim);

        /// <summary>
        /// Get a Lancamento by its Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Lancamento?> ObterPorIdAsync(Guid id);

        /// <summary>
        /// Remove a Lancamento entry.
        /// </summary>
        /// <param name="lancamento"></param>
        /// <returns></returns>
        Task RemoveAsync(Lancamento lancamento);

        /// <summary>
        /// update a lancamento
        /// </summary>
        /// <param name="lancamento"></param>
        /// <returns></returns>
        Task EditAsync(Lancamento lancamento);
        
        /// <summary>
        /// Asynchronously retrieves a financial summary.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the financial summary.</returns>
        Task<ResumoFinanceiro> ObterResumoAsync();

        /// <summary>
        /// Asynchronously retrieves the total values grouped by category.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of category report
        /// totals.</returns>
        Task<IEnumerable<RelatorioCategoria>> ObterTotaisCategoriaAsync();

        /// <summary>
        /// Asynchronously retrieves categories matching the specified category ID.
        /// </summary>
        /// <param name="categoriaId">The unique identifier of the category to search for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of matching
        /// categories.</returns>
        Task<IEnumerable<Categoria>> CategoriaExisteAsync(Guid categoriaId);

        Task<Lancamento?> ObterLancamentoPorTipo(TipoLancamento tipo);
    }
}