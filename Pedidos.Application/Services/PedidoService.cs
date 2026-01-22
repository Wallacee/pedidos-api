using AutoMapper;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<PedidoService> _logger;

        public PedidoService(
            IPedidoRepository repository,
            IMapper mapper,
            ILogger<PedidoService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PedidoResponse> CriarAsync(CriarPedidoRequest request)
        {
            _logger.LogInformation("Iniciando criação de pedido para o cliente {Cliente}", request.ClienteNome);

            if (request.Itens == null || !request.Itens.Any())
            {
                _logger.LogWarning("Tentativa de criar pedido sem itens");
                throw new ArgumentException("Pedido deve conter ao menos um item");
            }

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

            _logger.LogInformation("Pedido {PedidoId} criado com sucesso", pedido.Id);

            return _mapper.Map<PedidoResponse>(pedido);
        }

        public async Task<PedidoResponse?> ObterPorIdAsync(Guid id)
        {
            _logger.LogInformation("Buscando pedido {PedidoId}", id);

            var pedido = await _repository.ObterPorIdAsync(id);

            if (pedido == null)
            {
                _logger.LogWarning("Pedido {PedidoId} não encontrado", id);
                return null;
            }

            return _mapper.Map<PedidoResponse>(pedido);
        }

        public async Task<IEnumerable<PedidoResponse>> ObterPorStatusAsync(
            StatusPedido? status,
            int page,
            int pageSize)
        {
            _logger.LogInformation(
                "Listando pedidos | Status: {Status} | Página: {Page} | Tamanho: {PageSize}",
                status, page, pageSize);

            var pedidos = status.HasValue
                ? await _repository.ObterPorStatusAsync(status.Value, page, pageSize)
                : await _repository.ObterTodosAsync(page, pageSize);

            return _mapper.Map<IEnumerable<PedidoResponse>>(pedidos);
        }

        public async Task<bool> CancelarAsync(Guid id)
        {
            _logger.LogInformation("Solicitação de cancelamento do pedido {PedidoId}", id);

            var pedido = await _repository.ObterPorIdAsync(id);

            if (pedido == null)
            {
                _logger.LogWarning("Tentativa de cancelar pedido inexistente {PedidoId}", id);
                return false;
            }

            pedido.Cancelar();

            await _repository.AtualizarAsync(pedido);

            _logger.LogInformation("Pedido {PedidoId} cancelado com sucesso", id);
            return true;
        }

        public async Task PagarAsync(Guid id)
        {
            _logger.LogInformation("Solicitação de pagamento do pedido {PedidoId}", id);

            var pedido = await _repository.ObterPorIdAsync(id)
                ?? throw new Exception("Pedido não encontrado.");

            pedido.Pagar();

            await _repository.AtualizarAsync(pedido);

            _logger.LogInformation("Pedido {PedidoId} marcado como PAGO", id);
        }
    }
}
