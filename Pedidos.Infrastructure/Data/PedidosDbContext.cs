using Microsoft.EntityFrameworkCore;
using Pedidos.Domain.Entities;

namespace Pedidos.Infrastructure.Data
{
    public class PedidosDbContext : DbContext
    {
        public DbSet<Pedido> Pedidos => Set<Pedido>();
        public DbSet<ItemPedido> ItensPedido => Set<ItemPedido>();

        public PedidosDbContext(DbContextOptions<PedidosDbContext> options)
            : base(options)
        {
        }
    }
}
