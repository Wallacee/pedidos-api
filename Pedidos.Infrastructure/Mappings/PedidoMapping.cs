using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pedidos.Domain.Entities;

namespace Pedidos.Infrastructure.Mappings
{
    public class PedidoMapping : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedidos");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.ClienteNome)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(p => p.DataCriacao)
                   .IsRequired();

            builder.Property(p => p.Status)
                   .IsRequired();

            builder.Property(p => p.ValorTotal)
                   .HasPrecision(18, 2);

            builder.OwnsMany(p => p.Itens, itens =>
            {
                itens.ToTable("ItensPedido");

                itens.WithOwner()
                     .HasForeignKey("PedidoId");

                itens.HasKey("Id");

                itens.Property(i => i.ProdutoNome)
                     .IsRequired()
                     .HasMaxLength(200);

                itens.Property(i => i.Quantidade)
                     .IsRequired();

                itens.Property(i => i.PrecoUnitario)
                     .HasPrecision(18, 2);
            });
        }
    }
}
