using Catalogue.Domaine.Entities;
using Catalogue.Domaine.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalogue.Infrastructure.Configurations
{
    public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.HasKey(v => v.Id);
            builder.HasIndex(v => new { v.SKU, v.ProductId }).IsUnique(); 
            // Here we have Sku as Value object so EF cannot map it automaticly, so we use this method 
            // .HasConversion(1) sku => sku.Sku, 2) value => new Sku(value));
            // 1) Is how Ef map the data from Object to DB (create)
            // 2) Is how Ef map from DB to Object (read)
            builder.Property(v => v.SKU).IsRequired().HasMaxLength(50)
                .HasConversion(sku => sku.SKU, skuValue => new Sku(skuValue));
            builder.Property(v => v.Price).IsRequired()
                .HasPrecision(18, 2).
                HasConversion(price => price.Price, value => new Money(value));
            builder.Property(v => v.ProductId).IsRequired();
            builder.HasOne(v => v.Product).
                WithMany(p => p.ProductVariants).
                HasForeignKey(v => v.ProductId)
                .OnDelete(deleteBehavior: DeleteBehavior.Cascade);
        }
    }
}
