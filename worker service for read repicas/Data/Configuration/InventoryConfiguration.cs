using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using worker_service_for_read_repicas.Models;

namespace worker_service_for_read_repicas.Data.Configuration
{
    public class InventoryConfiguration : IEntityTypeConfiguration<ItemInventory>
    {
        public void Configure(EntityTypeBuilder<ItemInventory> builder)
        {
            builder.HasKey(p => p.ProductId);

            builder.Property(p => p.ProductName)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(p => p.Price)
                .HasColumnType("decimal(12,2)");
        }
    }
}
