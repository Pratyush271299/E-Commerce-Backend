using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Data.Config
{
    public class AllProductConfiguration : IEntityTypeConfiguration<AllProduct>
    {
        public void Configure(EntityTypeBuilder<AllProduct> builder)
        {
            builder.ToTable("AllProducts");
            builder.HasKey(cp => cp.Id);
            builder.Property(cp => cp.Id).UseIdentityColumn();

            //builder.Property(cp => cp.Title).IsRequired();
            //builder.Property(cp => cp.Category).IsRequired();
            //builder.Property(cp => cp.NewPrice).IsRequired();
            //builder.Property(cp => cp.OldPrice).IsRequired();
            //builder.Property(cp => cp.ImageUrl).IsRequired();
        }
    }
}
