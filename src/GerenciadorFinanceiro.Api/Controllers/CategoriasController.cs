using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GerenciadorFinanceiro.Domain.Entities;
using GerenciadorFinanceiro.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using GerenciadorFinanceiro.Domain.Interfaces;

namespace GerenciadorFinanceiro.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepository _repository;

        public CategoriasController(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodasCategorias()
        {
            var categorias = await _repository.ObterTodosAsync();
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterCategoriaPorId([FromRoute] Guid id)
        {
            var categoria = await _repository.ObterPorIdAsync(id);
            if (categoria == null)
            {
                return NotFound("Categoria não encontrada");
            }
            return Ok(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarCategoria([FromBody] CriarCategoriaRequest request)
        {
            var categoria = new Categoria(request.Nome, request.OrcamentoMensal);

            await _repository.AdicionarAsync(categoria);
            return Created("", categoria);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditarCategoria([FromRoute] Guid id, [FromBody] CriarCategoriaRequest request)
        {
            var categoria = await _repository.ObterPorIdAsync(id);
            if (categoria == null)
            {
                return NotFound("Categoria não encontrada");
            }
            categoria.Atualizar(request.Nome, request.OrcamentoMensal);

            await _repository.EditAsync(categoria);
            return Ok(categoria);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverCategoria([FromRoute] Guid id)
        {
            var categoria = await _repository.ObterPorIdAsync(id);
            if (categoria == null)
            {
                return NotFound("Categoria não encontrada");
            }
            await _repository.RemoveAsync(categoria);
            return NoContent();
        }
    }

    public record CriarCategoriaRequest(string Nome, decimal OrcamentoMensal);
}