using Catalogue.Domaine.Entities;
using Catalogue.Domaine.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalogue.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .ToTable(t => t
                .HasCheckConstraint("CK_Product_Status", "[Status] IN ('Activate', 'Inactivate')"));
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasMaxLength(150).IsRequired();
            builder.Property(p => p.CompanyId).IsRequired();
            builder.Property(p => p.Status).HasMaxLength(20).HasDefaultValue(ProductStatus.Activate)
                .HasConversion<string>();
            builder.HasOne(p => p.Company)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CompanyId)
                // Cannot delete Company who contain Products
                .OnDelete(deleteBehavior: DeleteBehavior.Restrict);
        }
    }
}
