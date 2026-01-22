using AutoMapper;
using Pedidos.Application.DTOs;
using Pedidos.Domain.Entities;

namespace Pedidos.Application.Mappings
{
    public class PedidoProfile : Profile
    {
        public PedidoProfile()
        {
            // Domain -> Response
            CreateMap<Pedido, PedidoResponse>()
                .ForMember(dest => dest.Status,
                           opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<ItemPedido, ItemPedidoResponse>();

            // Request -> Domain (USADO COM CUIDADO)
            CreateMap<ItemPedidoRequest, ItemPedido>();
        }
    }
}
