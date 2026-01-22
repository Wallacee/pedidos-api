using FluentAssertions;
using FluentValidation.TestHelper;
using Pedidos.Application.DTOs;
using Pedidos.Application.Validators;
using Pedidos.Domain.Entities;
using Pedidos.Domain.Enum;
using Pedidos.Domain.Exceptions;

namespace Pedidos.Tests.Domain
{
    public class PedidoTests
    {
        [Fact]
        public void CriarPedido_DeveCriarComStatusNovo()
        {
            var clienteNome = "Cliente Teste";

            var pedido = new Pedido(clienteNome);

            pedido.Status.Should().Be(StatusPedido.Novo);
            pedido.ClienteNome.Should().Be(clienteNome);
            pedido.Itens.Should().BeEmpty();
        }

        [Fact]
        public void CriarPedido_DeveCalcularValorTotalComBaseNosItens()
        {
            var pedido = new Pedido("Cliente Teste");

            pedido.AdicionarItem("Produto A", 2, 10);
            pedido.AdicionarItem("Produto B", 1, 20);

            var valorTotal = pedido.ValorTotal;

            valorTotal.Should().Be(40);
        }

        [Fact]
        public void CriarPedido_DeveFalharQuandoClienteNomeForVazio()
        {
            var validator = new CriarPedidoRequestValidator();

            var request = new CriarPedidoRequest
            {
                ClienteNome = "",
                Itens = new List<ItemPedidoRequest>
        {
            new() { ProdutoNome = "Produto 1", Quantidade = 5, PrecoUnitario = 10 }
        }
            };

            var result = validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor("ClienteNome");
        }

        [Fact]
        public void CriarPedido_DeveFalharQuandoListaItensForVazia()
        {
            var validator = new CriarPedidoRequestValidator();

            List<ItemPedidoRequest> listasItensVazio = [];

            var request = new CriarPedidoRequest
            {
                ClienteNome = "Cliente",
                Itens = listasItensVazio
            };

            var result = validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor("Itens");
        }

        [Fact]
        public void CriarPedido_DeveFalharQuandoProdutoNomeForVazio()
        {
            var validator = new CriarPedidoRequestValidator();

            var request = new CriarPedidoRequest
            {
                ClienteNome = "Cliente",
                Itens = new List<ItemPedidoRequest>
        {
            new() { ProdutoNome = "", Quantidade = 5, PrecoUnitario = 10 }
        }
            };

            var result = validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor("Itens[0].ProdutoNome");
        }

        [Fact]
        public void CriarPedido_DeveFalharQuandoQuantidadeForZero()
        {
            var validator = new CriarPedidoRequestValidator();

            var request = new CriarPedidoRequest
            {
                ClienteNome = "Cliente",
                Itens = new List<ItemPedidoRequest>
        {
            new() { ProdutoNome = "Produto", Quantidade = 0, PrecoUnitario = 10 }
        }
            };

            var result = validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor("Itens[0].Quantidade");
        }

        [Fact]
        public void CriarPedido_DeveFalharQuandoPrecoUnitarioForZero()
        {
            var validator = new CriarPedidoRequestValidator();

            var request = new CriarPedidoRequest
            {
                ClienteNome = "Cliente",
                Itens = new List<ItemPedidoRequest>
        {
            new() { ProdutoNome = "Produto", Quantidade = 5, PrecoUnitario = 0 }
        }
            };

            var result = validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor("Itens[0].PrecoUnitario");
        }


        [Fact]
        public void PagarPedido_DeveLancarExcecaoQuandoPedidoEstiverPago()
        {
            var pedido = new Pedido("Cliente Teste");

            pedido.AdicionarItem("Produto 1", 2, 50);

            pedido.Pagar();

            Action act = () => pedido.Cancelar();

            act.Should()
                .Throw<PedidoPagoNaoPodeSerCanceladoException>()
                .WithMessage("Pedido pago não pode ser cancelado");
        }

        [Fact]
        public void PagarPedido_DeveLancarExcecaoQuandoPedidoEstiverCancelado()
        {

            var pedido = new Pedido("Cliente Teste");
            pedido.AdicionarItem("Produto", 1, 10);
            pedido.Cancelar();

            Action act = () => pedido.Pagar();

            act.Should()
                .Throw<PedidoCanceladoNaoPodeSerPagoException>()
                .WithMessage("Pedido cancelado não pode ser pago");
        }
        [Fact]
        public void PagarPedido_DeveAlterarStatusParaPagoQuandoPedidoForValido()
        {
            var pedido = new Pedido("Cliente Teste");
            pedido.AdicionarItem("Produto", 2, 15);

            pedido.Pagar();

            pedido.Status.Should().Be(StatusPedido.Pago);
        }
    }
}
