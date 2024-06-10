using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Configuration
{
    public class CartDetail_Config : IEntityTypeConfiguration<CartDetail>
    {
        public void Configure(EntityTypeBuilder<CartDetail> builder)
        {
            builder.HasKey(a => a.Id);
            builder.HasOne(a => a.Cart).WithMany(a => a.CartDetails).HasForeignKey(a => a.CartID);
            builder.HasOne(a => a.Product).WithMany(a => a.CartDetails).HasForeignKey(a => a.ProductId);
        }
    }
}
