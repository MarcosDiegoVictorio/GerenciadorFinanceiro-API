# Estágio 1: Build (Compilação)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copia tudo e restaura as dependências
COPY . ./
RUN dotnet restore

# Publica a API em modo Release
RUN dotnet publish src/GerenciadorFinanceiro.Api/GerenciadorFinanceiro.Api.csproj -c Release -o out

# Estágio 2: Runtime (Rodar a aplicação)
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# Configura a porta que o Render usa (geralmente a variável PORT ou 8080)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# O comando que inicia o site
ENTRYPOINT ["dotnet", "GerenciadorFinanceiro.Api.dll"]