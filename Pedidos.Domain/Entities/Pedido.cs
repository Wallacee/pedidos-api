using Pedidos.Domain.Enum;
using Pedidos.Domain.Exceptions;

namespace Pedidos.Domain.Entities
{
    public class Pedido
    {
        public Guid Id { get; private set; }
        public string ClienteNome { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public StatusPedido Status { get; private set; }
        public decimal ValorTotal { get; private set; }

        public List<ItemPedido> Itens { get; private set; } = new();

        protected Pedido() { } // EF

        public Pedido(string clienteNome)
        {
            Id = Guid.NewGuid();
            ClienteNome = clienteNome;
            DataCriacao = DateTime.UtcNow;
            Status = StatusPedido.Novo;
        }

        public void AdicionarItem(string produtoNome, int quantidade, decimal precoUnitario)
        {
            
            Itens.Add(new ItemPedido(
                produtoNome,
                quantidade,
                precoUnitario
            ));

            RecalcularValor();
        }

        public void Cancelar()
        {
            if (Status == StatusPedido.Pago)
                throw new PedidoPagoNaoPodeSerCanceladoException();

            if (Status == StatusPedido.Cancelado)
                throw new PedidoCancelamentoDuploException();

                Status = StatusPedido.Cancelado;
        }

        public void Pagar()
        {
            if (Status == StatusPedido.Pago)
                throw new PedidoPagamentoDuploException();

            if (Status == StatusPedido.Cancelado)
                throw new PedidoCanceladoNaoPodeSerPagoException();

            Status = StatusPedido.Pago;
        }

        private void RecalcularValor()
        {
            ValorTotal = Itens.Sum(i => i.Quantidade * i.PrecoUnitario);
        }
    }
}
