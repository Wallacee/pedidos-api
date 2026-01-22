using FluentValidation;
using Pedidos.Application.DTOs;

namespace Pedidos.Application.Validators
{
    public class ItemPedidoRequestValidator : AbstractValidator<ItemPedidoRequest>
    {
        public ItemPedidoRequestValidator()
        {
            RuleFor(x => x.ProdutoNome)
                .NotEmpty();

            RuleFor(x => x.Quantidade)
                .GreaterThan(0);

            RuleFor(x => x.PrecoUnitario)
                .GreaterThan(0);
        }
    }
}
