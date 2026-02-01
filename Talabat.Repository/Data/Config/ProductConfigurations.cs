using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data.Config
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p=>p.Name).IsRequired()
                   .HasMaxLength(200);
            builder.Property(p => p.Description).IsRequired();
            builder.Property(p => p.PictureUrl).IsRequired();
            builder.Property(p => p.Price)
                   .HasColumnType("decimal(18,2)");
            builder.HasOne(p => p.Brand).WithMany()
                   .HasForeignKey(p => p.BrandId)
                   /*.OnDelete(DeleteBehavior.Cascade)*/;
            builder.HasOne(p => p.Category).WithMany()
                   .HasForeignKey(p => p.ProductCategoryId);
        }
    }
}
