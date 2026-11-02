using Catalogue.Domaine.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalogue.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasMaxLength(150).IsRequired();
            builder.Property(p => p.CompanyId).IsRequired();
            builder.HasOne(p => p.Company)
                .WithMany()
                .HasForeignKey(p => p.CompanyId)
                // Cannot delete Company who contain Products
                .OnDelete(deleteBehavior: DeleteBehavior.Restrict);
        }
    }
}
