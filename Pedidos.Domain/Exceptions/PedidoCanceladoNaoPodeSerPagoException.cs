namespace Pedidos.Domain.Exceptions
{
    public class PedidoCanceladoNaoPodeSerPagoException : BusinessException
    {
        public PedidoCanceladoNaoPodeSerPagoException()
            : base(
                "PEDIDO_CANCELADO_PAGO",
                "Pedido cancelado não pode ser pago")
        {
        }
    }
}
