using Microsoft.AspNetCore.Mvc;
using GerenciadorFinanceiro.Domain.Interfaces;
using GerenciadorFinanceiro.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace GerenciadorFinanceiro.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LancamentosController : ControllerBase
    {
        private readonly ILancamentoRepository _repository;

        public LancamentosController(ILancamentoRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        public async Task<IActionResult> ObterTodosLancamentos()
        {
            var lancamentos = await _repository.ObterTodosAsync();
            return Ok(lancamentos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterLancamentoById([FromRoute] Guid id)
        {
            try
            {
                var lancamento = await _repository.ObterPorIdAsync(id);
                if (lancamento == null)
                {
                    return NotFound("Lançamento não encontrado");
                }
                return Ok(lancamento);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("periodo")]
        public async Task<IActionResult> ObterLancamentoByDateTime([FromQuery] DateTime inicio, [FromQuery] DateTime fim)
        {
            try
            {
                var lancamento = await _repository.ObterPorPeriodoAsync(inicio, fim);
                if (!lancamento.Any())
                {
                    return NotFound("Lançamento não encontrado");
                }
                return Ok(lancamento);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("saldo")]
        public async Task<IActionResult> ObterSaldo()
        {
            var resumo = await _repository.ObterResumoAsync();
            return Ok(resumo);
        }

        [HttpGet("relatorio-categorias")]
        public async Task<IActionResult> ObterRelatorioCategorias()
        {
            var relatorio = await _repository.ObterTotaisCategoriaAsync();
            return Ok(relatorio);
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarLancamento([FromBody] CriarLancamentoRequest request)
        {
            try
            {
                // Verifica se a categoria existe antes de criar (opcional, mas bom)
                var existe = await _repository.CategoriaExisteAsync(request.CategoriaId);

                if (!existe.Any())
                {
                    return BadRequest("Categoria inválida ou não existente");
                }

                // Aqui transformamos o DTO (dados da tela) na Entidade (regras de negócio)
                var lancamento = new Lancamento(request.Descricao, request.Valor, request.Tipo, request.CategoriaId);

                await _repository.AdicionarAsync(lancamento);

                // Retorna 201 Created
                return Created("", lancamento);
            }
            catch (Exception ex)
            {
                // Se a regra de negócio falhar (ex: valor negativo), devolve erro 400
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverLancamento([FromRoute] Guid id)
        {
            try
            {
                var lancamento = await _repository.ObterPorIdAsync(id);
                if (lancamento == null)
                {
                    return NotFound("Lançamento não encontrado");
                }
                await _repository.RemoveAsync(lancamento);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditarLancamento([FromRoute] Guid id, [FromBody] CriarLancamentoRequest request)
        {
            try
            {
                var lancamento = await _repository.ObterPorIdAsync(id);
                if (lancamento == null)
                {
                    return NotFound("Lançamento não encontrado");
                }

                // Atualiza os campos do lançamento
                lancamento.Atualizar(request.Descricao, request.Valor, request.Tipo, request.CategoriaId);

                await _repository.EditAsync(lancamento);
                return Ok(lancamento);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    // DTO: Um objeto simples apenas para receber os dados do JSON
    // TipoLancamento: 1 = Receita, 2 = Despesa
    public record CriarLancamentoRequest(string Descricao, decimal Valor, TipoLancamento Tipo, Guid CategoriaId);
}