namespace Pedidos.Domain.Entities
{
    public class ItemPedido
    {
        public Guid Id { get; private set; }
        public string ProdutoNome { get; private set; } = string.Empty;
        public int Quantidade { get; private set; }
        public decimal PrecoUnitario { get; private set; }

        public decimal Subtotal => Quantidade * PrecoUnitario;

        protected ItemPedido() { }

        public ItemPedido(string produtoNome, int quantidade, decimal precoUnitario)
        {
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
        }
    }
}
