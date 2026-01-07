namespace GerenciadorFinanceiro.Domain.Entities
{
    public class Categoria
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }

        protected Categoria() { }

        public Categoria(string nome)
        {
            Id = Guid.NewGuid();
            Nome = nome;

            if (string.IsNullOrWhiteSpace(nome))
                throw new Exception("O nome da categoria é obrigatório.");
        }

        public void Atualizar(string nome)
        {
            Nome = nome;
            if (string.IsNullOrWhiteSpace(nome))
                throw new Exception("O nome da categoria é obrigatório.");
        }
    }
}
