using Pedidos.Domain.Entities;
using Pedidos.Domain.Enum;

namespace Pedidos.Application.Interfaces
{
    public interface IPedidoRepository
    {
        Task AdicionarAsync(Pedido pedido);
        Task AtualizarAsync(Pedido pedido);
        Task<Pedido?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Pedido>> ObterPorStatusAsync(StatusPedido status, int page, int pageSize);
        Task<IEnumerable<Pedido>> ObterTodosAsync(int page, int pageSize);

    }
}
