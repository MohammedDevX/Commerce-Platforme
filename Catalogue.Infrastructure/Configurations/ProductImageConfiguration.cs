using Catalogue.Domaine.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalogue.Infrastructure.Configurations
{
    internal class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>

    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).ValueGeneratedNever();
            builder.Property(i => i.FileKey).IsRequired();
            builder.Property(i => i.ContentType).IsRequired();
            builder.Property(i => i.IsPrimary).IsRequired();
            builder.HasOne(i => i.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
