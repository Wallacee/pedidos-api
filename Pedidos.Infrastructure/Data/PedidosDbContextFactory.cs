using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Pedidos.Infrastructure.Data
{
    public class PedidosDbContextFactory
    : IDesignTimeDbContextFactory<PedidosDbContext>
    {
        public PedidosDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PedidosDbContext>();

            optionsBuilder.UseSqlite(
                "Data Source=pedidos.db"
            );

            return new PedidosDbContext(optionsBuilder.Options);
        }
    }
}
