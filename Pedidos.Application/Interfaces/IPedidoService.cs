using Pedidos.Application.DTOs;
using Pedidos.Domain.Enum;

namespace Pedidos.Application.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoResponse> CriarAsync(CriarPedidoRequest request);
        Task<PedidoResponse?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<PedidoResponse>> ObterPorStatusAsync(StatusPedido? status, int page, int pageSize);
        Task<bool> CancelarAsync(Guid id);
        Task PagarAsync(Guid id);
    }
}
