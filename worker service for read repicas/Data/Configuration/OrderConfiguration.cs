using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using worker_service_for_read_repicas.Models;

namespace worker_service_for_read_repicas.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(o => o.TotalAmount)
                .HasColumnType("decimal(12,2)");

            builder.Property(o => o.CreatedAt)
                .HasDefaultValueSql("now()");

            builder.Property(o => o.UpdatedAt)
                .HasDefaultValueSql("now()");

            builder.HasMany(o => o.Items)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);
        }
    }
}
