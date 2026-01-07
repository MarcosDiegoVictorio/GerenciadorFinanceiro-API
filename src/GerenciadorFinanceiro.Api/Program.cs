using GerenciadorFinanceiro.Domain.Interfaces;
using GerenciadorFinanceiro.Infrastructure.Repositories;
using GerenciadorFinanceiro.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Primeiro registramos o Contexto do Banco
// Isso ensina o .NET a criar o "FinanceiroDbContext" usando a string de conexão do appsettings.json
builder.Services.AddDbContext<FinanceiroDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Depois registramos o Repositório
// Como o Repositório pede o Contexto no construtor, o passo 1 precisa existir.
builder.Services.AddScoped<ILancamentoRepository, LancamentoRepository>();

var app = builder.Build();

// Configura o pipeline de requisições HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();