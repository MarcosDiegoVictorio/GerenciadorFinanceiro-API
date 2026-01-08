using GerenciadorFinanceiro.Api.Services;
using GerenciadorFinanceiro.Domain.Interfaces;
using GerenciadorFinanceiro.Infrastructure.Context;
using GerenciadorFinanceiro.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GerenciadorFinanceiro.Api", Version = "v1" });

    // Define que usamos segurança via JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT desta maneira: Bearer {seu_token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// 1. Primeiro registramos o Contexto do Banco
// Isso ensina o .NET a criar o "FinanceiroDbContext" usando a string de conexão do appsettings.json
builder.Services.AddDbContext<FinanceiroDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Depois registramos o Repositório
// Como o Repositório pede o Contexto no construtor, o passo 1 precisa existir.
builder.Services.AddScoped<ILancamentoRepository, LancamentoRepository>();

builder.Services.AddScoped<TokenService>();

// 2. Configura a Autenticação JWT
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:ChaveSecreta"]);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configura o pipeline de requisições HTTP.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// --- BLOCO DE AUTO-MIGRAÇÃO ---
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FinanceiroDbContext>();
    try
    {
        // Isso aplica qualquer migration pendente automaticamente ao iniciar
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Se der erro, só loga (útil para debug)
        Console.WriteLine($"Erro ao aplicar migrações: {ex.Message}");
    }
}

app.Run();