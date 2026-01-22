using AutoMapper;
using Pedidos.Application.DTOs;
using Pedidos.Application.Interfaces;
using Pedidos.Domain.Entities;
using Pedidos.Domain.Enum;

namespace Pedidos.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _repository;
        private readonly IMapper _mapper;

        public PedidoService(IPedidoRepository repository,   IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PedidoResponse> CriarAsync(CriarPedidoRequest request)
        {
            if (request.Itens == null || !request.Itens.Any())
                throw new ArgumentException("Pedido deve conter ao menos um item");

            var pedido = new Pedido(request.ClienteNome);

            foreach (var item in request.Itens)
            {
                pedido.AdicionarItem(
                    item.ProdutoNome,
                    item.Quantidade,
                    item.PrecoUnitario
                );
            }

            await _repository.AdicionarAsync(pedido);

            return _mapper.Map<PedidoResponse>(pedido);
        }

        public async Task<PedidoResponse?> ObterPorIdAsync(Guid id)
        {
            var pedido = await _repository.ObterPorIdAsync(id);
            return pedido == null ? null : _mapper.Map<PedidoResponse>(pedido);
        }

        public async Task<IEnumerable<PedidoResponse>> ObterPorStatusAsync(
    StatusPedido? status,
    int page,
    int pageSize)
        {
            var pedidos = status.HasValue
                ? await _repository.ObterPorStatusAsync(status.Value, page, pageSize)
                : await _repository.ObterTodosAsync(page, pageSize);

            return _mapper.Map<IEnumerable<PedidoResponse>>(pedidos);
        }

        public async Task<bool> CancelarAsync(Guid id)
        {
            var pedido = await _repository.ObterPorIdAsync(id);

            if (pedido == null)
                return false;

            pedido.Cancelar();

            await _repository.AtualizarAsync(pedido);
            return true;
        }

        public async Task PagarAsync(Guid id)
        {
            var pedido = await _repository.ObterPorIdAsync(id) ?? throw new Exception("Pedido não encontrado.");
            pedido.Pagar();

            await _repository.AtualizarAsync(pedido);
        }
    }
}
