using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GerenciadorFinanceiro.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace GerenciadorFinanceiro.Api.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
         
        public string GerarToken(Usuario usuario)
        {
            // 1. Pega a chave secreta do appsettings.json
            var keyString = _configuration["Jwt:ChaveSecreta"] ?? throw new Exception("Chave JWT não encontrada!");
            var key = Encoding.ASCII.GetBytes(keyString);

            // 2. Define os dados que vão DENTRO do token (Claims)
            var tokenConfig = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Email, usuario.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(2), // Token vale por 2 horas
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            // 3. Gera o Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfig);

            return tokenHandler.WriteToken(token);
        }
    }
}