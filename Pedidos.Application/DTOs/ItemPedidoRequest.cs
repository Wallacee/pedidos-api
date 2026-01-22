namespace Pedidos.Application.DTOs
{
    public class ItemPedidoRequest
    {
        public string ProdutoNome { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }

}
