using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GerenciadorFinanceiro.Domain.Entities;
using GerenciadorFinanceiro.Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;

namespace GerenciadorFinanceiro.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriasController : ControllerBase
    {
        private readonly FinanceiroDbContext _context;

        public CategoriasController(FinanceiroDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodas()
        {
            var categorias = await _context.Categorias.ToListAsync();
            return Ok(categorias);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarCategoriaRequest request)
        {
            var categoria = new Categoria(request.Nome);
            await _context.Categorias.AddAsync(categoria);
            await _context.SaveChangesAsync();
            return Created("", categoria);
        }
    }

    public record CriarCategoriaRequest(string Nome);
}