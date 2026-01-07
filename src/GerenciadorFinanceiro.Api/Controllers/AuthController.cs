using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GerenciadorFinanceiro.Domain.Entities;
using GerenciadorFinanceiro.Infrastructure.Context;
using GerenciadorFinanceiro.Api.Services;
using BCrypt.Net;

namespace GerenciadorFinanceiro.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly FinanceiroDbContext _context;
        private readonly TokenService _tokenService;

        public AuthController(FinanceiroDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistroRequest request)
        {
            // Verifica se email já existe
            if (await _context.Usuarios.AnyAsync(u => u.Email == request.Email))
                return BadRequest("Email já cadastrado.");

            // Criptografa a senha antes de salvar
            var senhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha);

            var usuario = new Usuario(request.Nome, request.Email, senhaHash);

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            return Ok("Usuário cadastrado com sucesso!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Busca usuário pelo email
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            // Se não achou OU a senha não bate com o Hash
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash))
                return Unauthorized("Email ou senha inválidos");

            // Se deu tudo certo, gera o Token
            var token = _tokenService.GerarToken(usuario);

            return Ok(new { token = token });
        }
    }

    // DTOs (Data Transfer Objects) simples para receber os dados
    public record RegistroRequest(string Nome, string Email, string Senha);
    public record LoginRequest(string Email, string Senha);
}