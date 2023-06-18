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
    class OrderItemEntityTypeConfiguration
 : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem>
        orderItemConfiguration)
        {
            orderItemConfiguration.ToTable("OrderItems",
            OrderingContext.DEFAULT_SCHEMA);
            orderItemConfiguration.HasKey(o => o.Id);
            orderItemConfiguration.Property(o => o.Id)
            .UseHiLo("orderitemseq");
            orderItemConfiguration.Property(o => o.Id);
            orderItemConfiguration.Property<int>("OrderId")
            .IsRequired();
            orderItemConfiguration
            .Property<decimal>("_discount")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("Discount")
            .HasColumnType("decimal(18,2)")
            .IsRequired();
            orderItemConfiguration.Property<int>("ProductId")
            .IsRequired();
            orderItemConfiguration
            .Property<string>("_productName")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("ProductName")
            .IsRequired();
            orderItemConfiguration
            .Property<decimal>("_unitPrice")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("UnitPrice")
            .HasColumnType("decimal(18,2)")
            .IsRequired();
            orderItemConfiguration
            .Property<int>("_units")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("Units")
            .IsRequired();
            orderItemConfiguration
            .Property<string>("_pictureUrl")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("PictureUrl")
            .IsRequired(false);
            orderItemConfiguration.Ignore(b => b.DomainEvents);
        }
    }
}
