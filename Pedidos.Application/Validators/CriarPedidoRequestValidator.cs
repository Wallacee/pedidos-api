using FluentValidation;
using Pedidos.Application.DTOs;

namespace Pedidos.Application.Validators
{
    public class CriarPedidoRequestValidator : AbstractValidator<CriarPedidoRequest>
    {
        public CriarPedidoRequestValidator()
        {
            RuleFor(x => x.ClienteNome)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Itens)
                .NotNull()
                .NotEmpty();

            RuleForEach(x => x.Itens)
                .SetValidator(new ItemPedidoRequestValidator());
        }
    }
}
