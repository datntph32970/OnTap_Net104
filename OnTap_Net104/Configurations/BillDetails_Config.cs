using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnTap_Net104.Models;

namespace OnTap_Net104.Configurations
{
    public class BillDetails_Config : IEntityTypeConfiguration<BillDetail>
    {
        public void Configure(EntityTypeBuilder<BillDetail> builder)
        {
            builder.HasKey(a => a.Id);
            builder.HasOne(a => a.Bill).WithMany(a => a.BillDetails).HasForeignKey(a => a.BillId);
            builder.HasOne(a => a.Product).WithMany(a => a.BillDetails).HasForeignKey(a => a.ProductId);
        }
    }
}
