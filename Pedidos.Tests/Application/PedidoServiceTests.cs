using AutoMapper;
using FluentAssertions;
using Moq;
using Pedidos.Application.DTOs;
using Pedidos.Application.Interfaces;
using Pedidos.Application.Services;
using Pedidos.Domain.Entities;
using Pedidos.Domain.Enum;

namespace Pedidos.Tests.Application
{
    public class PedidoServiceTests
    {
        private readonly Mock<IPedidoRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PedidoService _service;

        public PedidoServiceTests()
        {
            _repositoryMock = new Mock<IPedidoRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new PedidoService(
                _repositoryMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task CriarPedidoAsync_DeveLancarExcecaoQuandoNaoHouverItens()
        {
            // Arrange
            var request = new CriarPedidoRequest
            {
                ClienteNome = "Cliente Teste",
                Itens = []
            };

            // Act
            Func<Task> act = async () => await _service.CriarAsync(request);

            // Assert
            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage("Pedido deve conter ao menos um item");
        }

        [Fact]
        public async Task CriarPedidoAsync_DeveCriarPedidoQuandoRequestForValido()
        {
            // Arrange
            var request = new CriarPedidoRequest
            {
                ClienteNome = "Cliente Teste",
                Itens =
                [
                    new ItemPedidoRequest
                    {
                        ProdutoNome = "Produto A",
                        Quantidade = 2,
                        PrecoUnitario = 10
                    }
                ]
            };

            var response = new PedidoResponse
            {
                ClienteNome = "Cliente Teste"
            };

            _mapperMock
                .Setup(m => m.Map<PedidoResponse>(It.IsAny<Pedido>()))
                .Returns(response);

            // Act
            var result = await _service.CriarAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.ClienteNome.Should().Be("Cliente Teste");

            _repositoryMock.Verify(
                r => r.AdicionarAsync(It.IsAny<Pedido>()),
                Times.Once
            );
        }

        [Fact]
        public async Task BuscarPorIdAsync_DeveRetornarNullQuandoPedidoNaoExistir()
        {
            // Arrange
            _repositoryMock
                .Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Pedido?)null);

            // Act
            var result = await _service.ObterPorIdAsync(Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task BuscarPorIdAsync_DeveRetornarPedidoQuandoExistir()
        {
            // Arrange
            var pedido = new Pedido("Cliente Teste");

            var response = new PedidoResponse
            {
                ClienteNome = "Cliente Teste"
            };

            _repositoryMock
                .Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(pedido);

            _mapperMock
                .Setup(m => m.Map<PedidoResponse>(pedido))
                .Returns(response);

            // Act
            var result = await _service.ObterPorIdAsync(Guid.NewGuid());

            // Assert
            result.Should().NotBeNull();
            result!.ClienteNome.Should().Be("Cliente Teste");
        }

        [Fact]
        public async Task BuscarTodosAsync_DeveBuscarPorStatusQuandoStatusForInformado()
        {
            // Arrange
            var pedidos = new List<Pedido>
            {
                new("Cliente 1"),
                new("Cliente 2")
            };

            _repositoryMock
                .Setup(r => r.ObterPorStatusAsync(StatusPedido.Novo, 1, 10))
                .ReturnsAsync(pedidos);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<PedidoResponse>>(pedidos))
                .Returns(
                [
                    new(),
                    new()
                ]);

            // Act
            var result = await _service.ObterPorStatusAsync(StatusPedido.Novo, 1, 10);

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task BuscarTodosAsync_DeveBuscarTodosQuandoStatusForNulo()
        {
            // Arrange
            var pedidos = new List<Pedido>
            {
                new("Cliente 1")
            };

            _repositoryMock
                .Setup(r => r.ObterTodosAsync(1, 10))
                .ReturnsAsync(pedidos);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<PedidoResponse>>(pedidos))
                .Returns(
                [
                    new()
                ]);

            // Act
            var result = await _service.ObterPorStatusAsync(null, 1, 10);

            // Assert
            result.Should().HaveCount(1);
        }
    

        [Fact]
        public async Task CancelarPedidoAsync_DeveCancelarPedidoQuandoStatusForNovo()
        {
            // Arrange
            var pedido = new Pedido("Cliente Teste");

            var repositoryMock = new Mock<IPedidoRepository>();
            repositoryMock
                .Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(pedido);

            var service = new PedidoService(repositoryMock.Object, null!);

            // Act
            await service.CancelarAsync(pedido.Id);

            // Assert
            pedido.Status.Should().Be(StatusPedido.Cancelado);
            repositoryMock.Verify(r => r.AtualizarAsync(pedido), Times.Once);
        }
    }
}
