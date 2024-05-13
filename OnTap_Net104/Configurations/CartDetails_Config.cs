using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnTap_Net104.Models;

namespace OnTap_Net104.Configurations
{
    public class CartDetails_Config : IEntityTypeConfiguration<CartDetail>
    {
        public void Configure(EntityTypeBuilder<CartDetail> builder)
        {
            builder.HasKey(a => a.Id);
            builder.HasOne(a => a.Cart).WithMany(a => a.CartDetails).HasForeignKey(a => a.CartID);
            builder.HasOne(a => a.Product).WithMany(a => a.CartDetails).HasForeignKey(a => a.ProductId);
        }
    }
}
