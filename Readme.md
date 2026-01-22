<div align="center">

# 🏦 Financeiro API - Backend

![.NET 8](https://img.shields.io/badge/.NET%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![SQLite](https://img.shields.io/badge/SQLite-003B57?style=for-the-badge&logo=sqlite&logoColor=white)
![Render](https://img.shields.io/badge/Render-46E3B7?style=for-the-badge&logo=render&logoColor=white)

<p>
  API RESTful desenvolvida em <strong>.NET 8</strong> para gerenciar dados financeiros pessoais. Este projeto serve como o backend para a aplicação web "Minha Carteira", fornecendo endpoints para transações, categorias e dados analíticos.
</p>

🔗 **Base URL (Produção):** `https://financeiro-api-pessoal.onrender.com/api`
📄 **Documentação Swagger:** `https://financeiro-api-pessoal.onrender.com/swagger`

</div>

---

## 🚀 Tecnologias Utilizadas

- **C# .NET 8** (ASP.NET Core Web API)
- **Entity Framework Core** (ORM)
- **SQLite** (Banco de Dados)
- **Swagger / OpenAPI** (Documentação)
- **Docker** (Containerização)
- **Render** (Deploy e Hospedagem)

## ⚙️ Funcionalidades da API

A API fornece recursos para:

- **Dashboard:** Cálculos consolidados de saldo, total de receitas, despesas e investimentos.
- **Lançamentos:** CRUD completo (Criar, Ler, Atualizar, Deletar) de movimentações financeiras.
  - Suporte a tipos: Receita, Despesa e Investimento 📈.
- **Categorias:** Gerenciamento de categorias de gastos com definição de orçamento mensal (Budget).
- **CORS Config:** Configurado para aceitar requisições do Frontend hospedado na Vercel.

## 🔌 Endpoints Principais

| Método | Rota | Descrição |
| :--- | :--- | :--- |
| `GET` | `/api/Lancamentos` | Lista todas as movimentações |
| `POST` | `/api/Lancamentos` | Cria uma nova movimentação |
| `PUT` | `/api/Lancamentos/{id}` | Atualiza uma movimentação existente |
| `DELETE` | `/api/Lancamentos/{id}` | Remove uma movimentação |
| `GET` | `/api/Categorias` | Lista todas as categorias |
| `POST` | `/api/Categorias` | Cria uma nova categoria |

*(Para ver todos os detalhes e testar as rotas, acesse o [Swagger](https://financeiro-api-pessoal.onrender.com/swagger))*

---

## 🛠️ Como rodar localmente

### Pré-requisitos
- [.NET SDK 8.0](https://dotnet.microsoft.com/download)
- SQL Server (ou LocalDB) instalado

### Passo a passo

1. **Clone o repositório**
   ```bash
   git clone https://github.com/MarcosDiegoVictorio/GerenciadorFinanceiro-API.git
   cd backend
   ```

2. **Configure o Banco de Dados**
   No arquivo `appsettings.json`, verifique a string de conexão:
   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=MinhaCarteiraDB;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

3. **Aplique as Migrations**
   Crie o banco de dados e as tabelas rodando o comando:
   ```bash
   dotnet ef database update
   ```

4. **Execute o projeto**
   ```bash
   dotnet run
   ```
   **A API estará disponível em `http://localhost:5000` (ou porta similar).**

### 🐳 Rodando com Docker

```bash
docker build -t financeiro-api .
docker run -p 5000:8080 financeiro-api
```

---

### 📦 Estrutura do Projeto
**O projeto segue uma arquitetura limpa e organizada:**

- **.Controllers:** Pontos de entrada da API (Endpoints).
- **.Models:** Entidades do banco de dados (Lancamento, Categoria).
- **.Data:** Contexto do Entity Framework e configurações de DB.
- **.DTOs:** Objetos de transferência de dados (Requests/Responses).

### 🤝 Integração com Frontend
**Este backend foi projetado para funcionar em conjunto com o frontend React:**
👉 **[https://github.com/MarcosDiegoVictorio/Web-Financeira](https://github.com/MarcosDiegoVictorio/Web-Financeira)**

<div align="center">

#### Desenvolvido por Marcos Diego Victorio

</div>