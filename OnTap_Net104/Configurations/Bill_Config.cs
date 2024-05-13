using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnTap_Net104.Models;

namespace OnTap_Net104.Configurations
{
    public class Bill_Config : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.Account).WithMany(p => p.Bills).HasForeignKey(p => p.Username);
        }
    }
}
