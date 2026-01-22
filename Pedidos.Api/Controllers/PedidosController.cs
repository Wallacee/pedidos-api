using Microsoft.AspNetCore.Mvc;
using Pedidos.Application.DTOs;
using Pedidos.Application.Interfaces;
using Pedidos.Domain.Enum;

namespace Pedidos.Api.Controllers
{
    [ApiController]
    [Route("pedidos")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _service;

        public PedidosController(IPedidoService service)
        {
            _service = service;
        }

        /// <summary>
        /// POST /pedidos
        /// Cria um pedido com itens
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CriarPedido([FromBody] CriarPedidoRequest request)
        {
            var pedidoId = await _service.CriarAsync(request);

            return Created($"/pedidos/{pedidoId}", new { id = pedidoId });
        }

        /// <summary>
        /// GET /pedidos/{id}
        /// Retorna pedido + itens
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> ObterPedidoPorId(Guid id)
        {
            var pedido = await _service.ObterPorIdAsync(id);

            if (pedido == null)
                return NotFound();

            return Ok(pedido);
        }

        /// <summary>
        /// GET /pedidos?status=Pago&page=1&pageSize=10
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ListarPedidos(
            [FromQuery] StatusPedido? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var pedidos = await _service.ObterPorStatusAsync(status, page, pageSize);
            return Ok(pedidos);
        }

        /// <summary>
        /// PUT /pedidos/{id}/cancelar
        /// </summary>
        [HttpPut("{id:guid}/cancelar")]
        public async Task<IActionResult> CancelarPedido(Guid id)
        {
            await _service.CancelarAsync(id);
            return NoContent();
        }

        /// <summary>
        /// PUT /pedidos/{id}/pagar
        /// Realiza o pagamento do pedido
        /// </summary>
        [HttpPut("{id:guid}/pagar")]
        public async Task<IActionResult> PagarPedido(Guid id)
        {
            await _service.PagarAsync(id);
            return NoContent();
        }

    }
}
