using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GerenciadorFinanceiro.Domain.Exceptions;

namespace GerenciadorFinanceiro.Api.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // Se NÃO for nossa exceção de domínio, deixa passar (vai dar erro 500 normal)
            if (exception is not DomainException)
            {
                return false;
            }

            _logger.LogWarning("Erro de validação: {Message}", exception.Message);

            // Monta a resposta bonitinha (RFC 7807 - Problem Details)
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Erro de Validação",
                Detail = exception.Message,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            // Escreve o JSON na resposta
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true; // Avisa o .NET que nós tratamos o erro
        }
    }
}