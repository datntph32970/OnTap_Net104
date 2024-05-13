using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnTap_Net104.Models;

namespace OnTap_Net104.Configurations
{
    public class Cart_Config : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(a => a.Username);
            builder.HasOne(a => a.Account).WithOne(a => a.Cart).HasForeignKey<Cart>(a => a.Username).HasConstraintName("FK_Cart_Account");
        }
    }
}
