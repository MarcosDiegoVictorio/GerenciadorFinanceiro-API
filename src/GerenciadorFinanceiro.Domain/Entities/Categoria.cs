namespace GerenciadorFinanceiro.Domain.Entities
{
    public class Categoria
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public decimal OrcamentoMensal { get; private set; }

        protected Categoria() { }

        public Categoria(string nome, decimal orcamentoMensal)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            OrcamentoMensal = orcamentoMensal;


            if (string.IsNullOrWhiteSpace(nome))
                throw new Exception("O nome da categoria é obrigatório.");

            if (orcamentoMensal < 0)
                throw new Exception("O orçamento mensal não pode ser negativo.");
        }

        public void Atualizar(string nome, decimal orcamentoMensal)
        {
            Nome = nome;
            OrcamentoMensal = orcamentoMensal;
            if (string.IsNullOrWhiteSpace(nome))
                throw new Exception("O nome da categoria é obrigatório.");

            if (orcamentoMensal < 0)
                throw new Exception("O orçamento mensal não pode ser negativo.");
        }
    }
}
