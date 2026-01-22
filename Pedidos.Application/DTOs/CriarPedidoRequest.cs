namespace Pedidos.Application.DTOs
{
    public class CriarPedidoRequest
    {
        public string ClienteNome { get; set; } = string.Empty;
        public List<ItemPedidoRequest> Itens { get; set; } = new();
    }

}
