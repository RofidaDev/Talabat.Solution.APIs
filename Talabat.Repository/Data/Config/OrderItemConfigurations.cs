using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.Config
{
    internal class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            //Product will map with Owner(OrderItem)
            builder.OwnsOne(OrderItem => OrderItem.Product, Product => Product.WithOwner());
            builder.Property(OI => OI.Price)
               .HasColumnType("decimal(18,2)");
        }
    }
}
