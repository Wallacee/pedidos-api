namespace Pedidos.Domain.Exceptions
{
    public class PedidoPagoNaoPodeSerCanceladoException
    : BusinessException
    {
        public PedidoPagoNaoPodeSerCanceladoException()
            : base(
                "PEDIDO_PAGO_CANCELADO",
                "Pedido pago não pode ser cancelado")
        {
        }
    }
}
