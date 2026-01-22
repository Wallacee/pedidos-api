namespace Pedidos.Domain.Exceptions
{
    public class PedidoCancelamentoDuploException : BusinessException
    {
        public PedidoCancelamentoDuploException()
            : base(
                "PEDIDO_CANCELAMENTO_DUPLO",
                "Pedido cancelado não pode ser cancelado novamente")
        {
        }
    }
}
