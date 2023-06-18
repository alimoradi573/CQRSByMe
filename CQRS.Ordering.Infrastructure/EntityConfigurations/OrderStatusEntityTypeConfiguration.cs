using CQRS.Ordering.Domain.AggregatesModel.OrderAggregate;
using CQRS.Ordering.Infrastructure.DataContexts;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Ordering.Infrastructure.EntityConfigurations
{
    class OrderStatusEntityTypeConfiguration
 : IEntityTypeConfiguration<OrderStatus>
    {
        public void Configure(EntityTypeBuilder<OrderStatus>
        orderStatusConfiguration)
        {
            orderStatusConfiguration.ToTable("Orderstatus",
            OrderingContext.DEFAULT_SCHEMA);
            orderStatusConfiguration.HasKey(o => o.Id);
            orderStatusConfiguration.Property(o => o.Id)
            .HasDefaultValueSql("1")
            .ValueGeneratedNever()
            .IsRequired();
            orderStatusConfiguration.Property(o => o.Name)
            .HasMaxLength(200)
            .IsRequired();
        }
    }
}
