using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IdentityFrameworkSportsstore.Models.Domain;

namespace IdentityFrameworkSportsstore.Data.Mappers
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");
            builder.HasKey(t => t.CategoryId);
            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);

            builder.HasMany(t => t.Products).
                WithOne(t => t.Category).
                IsRequired().
                OnDelete(DeleteBehavior.Restrict);
        }
    }
}