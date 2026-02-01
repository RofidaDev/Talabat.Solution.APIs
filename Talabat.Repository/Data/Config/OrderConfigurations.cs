using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.Config
{
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            //map address in table order(owner)
            builder.OwnsOne(O => O.ShippingAddress, ShippingAddress => ShippingAddress.WithOwner());  //1 to 1 [total]
            //store order status in DB as string - return from DB as orderStatus(string or int)
            builder.Property(O => O.Status)
                .HasConversion(
                 OStatus => OStatus.ToString(),
                 OStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatus)
                 );
            // ( 1(DeliveryMethod to many(Order) )  //By default
            // to map 1 to 1 add unique constraint :
            //builder.HasOne(O => O.DeliveryMethod).WithOne();  //or
            //builder.HasIndex(O => O.DeliveryMethod).IsUnique();
            builder.Property(O => O.SubTotal)
                .HasColumnType("decimal(18,2)");
            builder.HasOne(O => O.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);


        }
    }
}
