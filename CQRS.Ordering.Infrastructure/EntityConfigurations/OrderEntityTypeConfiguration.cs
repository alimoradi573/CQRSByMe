using CQRS.Ordering.Domain.AggregatesModel.BuyerAggregate;
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
    class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> orderConfiguration)
        {
            orderConfiguration.ToTable("Orders",
            OrderingContext.DEFAULT_SCHEMA);  

        orderConfiguration.HasKey(o => o.Id);

            orderConfiguration.Property(o => o.Id)
            .UseHiLo("orderseq", OrderingContext.DEFAULT_SCHEMA);

            orderConfiguration.OwnsOne(o => o.Address, a =>
            {
                a.WithOwner();
            });

            orderConfiguration.Ignore(b => b.DomainEvents);

            orderConfiguration
            .Property<int?>("_buyerId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("BuyerId")
            .IsRequired(false);

            orderConfiguration
            .Property<DateTime>("_orderDate")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("OrderDate")
            .IsRequired();

            orderConfiguration
            .Property<int>("_orderStatusId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("OrderStatusId")
            .IsRequired();

            orderConfiguration.Property<string>("Description").IsRequired(false);

            var navigation =orderConfiguration.Metadata.FindNavigation(nameof(Order.OrderItems));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
            orderConfiguration.HasOne<Buyer>()
            .WithMany()
            .IsRequired(false)
            // .HasForeignKey("BuyerId");
            .HasForeignKey("_buyerId");  
        orderConfiguration.HasOne(o => o.OrderStatus)
        .WithMany()
        // .HasForeignKey("OrderStatusId");
        .HasForeignKey("_orderStatusId");
        }

        
    }

}
