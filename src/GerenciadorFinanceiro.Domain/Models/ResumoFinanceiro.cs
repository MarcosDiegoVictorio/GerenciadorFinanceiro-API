namespace GerenciadorFinanceiro.Domain.Models
{
    //record carrega os dados de forma mais leve, porém os mesmos não podem ser alterados
    public record ResumoFinanceiro(decimal TotalReceitas, decimal TotalDespesas, decimal Saldo);
}
