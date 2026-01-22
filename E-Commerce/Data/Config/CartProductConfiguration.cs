using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Data.Config
{
    public class CartProductConfiguration : IEntityTypeConfiguration<CartProduct>
    {
        public void Configure(EntityTypeBuilder<CartProduct> builder)
        {
            builder.ToTable("CartProducts");
            builder.HasKey(cp => cp.Id);
            builder.Property(cp => cp.Id).UseIdentityColumn();

            builder.Property(cp => cp.Title).IsRequired();
            builder.Property(cp => cp.ImageUrl).IsRequired();
            builder.Property(cp => cp.Price).IsRequired();
            builder.Property(cp => cp.Quantity).IsRequired();
            builder.Property(cp => cp.Total).IsRequired();
            builder.Property(cp => cp.UserId).IsRequired();

            builder.HasOne(cp => cp.User)
                .WithMany(user => user.CartProducts)
                .HasForeignKey(cp => cp.UserId)
                .HasConstraintName("FK_CartProducts_Users");

            builder.HasOne(cp => cp.AllProduct)
                .WithMany(ap => ap.CartProducts)
                .HasForeignKey(cp => cp.AllProductId)
                .HasConstraintName("FK_CartProducts_AllProducts");
        }
    }
}
