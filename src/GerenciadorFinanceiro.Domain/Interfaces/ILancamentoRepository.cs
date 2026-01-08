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

        Task<ResumoFinanceiro> ObterResumoAsync();
        Task<IEnumerable<RelatorioCategoria>> ObterTotaisCategoriaAsync();
        Task<IEnumerable<Categoria>> CategoriaExisteAsync(Guid categoriaId);
    }
}