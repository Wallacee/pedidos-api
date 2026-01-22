namespace Pedidos.Domain.Exceptions
{
    public class PedidoPagamentoDuploException : BusinessException
    {
        public PedidoPagamentoDuploException()
            : base(
                "PEDIDO_PAGAMENTO_DUPLO",
                "Pedido pago não pode ser pago novamente.")
        {
        }
    }
}
