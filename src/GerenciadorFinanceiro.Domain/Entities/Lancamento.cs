using GerenciadorFinanceiro.Domain.Entities;
using System;

namespace GerenciadorFinanceiro.Domain.Entities
{
    // Enum para definir se é Receita ou Despesa
    public enum TipoLancamento
    {
        Receita = 1,
        Despesa = 2,
        Investimento = 3
    }

    public class Lancamento
    {
        public Guid Id { get; private set; }
        public string Descricao { get; private set; }
        public decimal Valor { get; private set; }
        public TipoLancamento Tipo { get; private set; }
        public DateTime DataLancamento { get; private set; }

        
        public Guid CategoriaId { get; private set; }
        public virtual Categoria? Categoria { get; private set; } 

        protected Lancamento() { }

        public Lancamento(string descricao, decimal valor, TipoLancamento tipo, Guid categoriaId)
        {
            Id = Guid.NewGuid();
            Descricao = descricao;
            Valor = valor;
            Tipo = tipo;
            CategoriaId = categoriaId;
            DataLancamento = DateTime.Now;

            Validar(); // Garante que não nasce com dados errados
        }
        public void Atualizar(string descrição, decimal valor, TipoLancamento tipo, Guid categoriaId)
        {
            Descricao = descrição;
            Valor = valor;
            Tipo = tipo;
            CategoriaId = categoriaId;

            Validar();
        }
        // Regras de Negócio
        private void Validar()
        {
            if (string.IsNullOrWhiteSpace(Descricao))
                throw new Exception("A descrição é obrigatória.");

            if (Valor <= 0)
                throw new Exception("O valor deve ser maior que zero.");

            if (CategoriaId == Guid.Empty)
                throw new Exception("Categoria inválida");
        }
    }
}
