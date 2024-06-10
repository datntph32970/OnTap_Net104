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
    public class Cart_Config : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {

            builder.HasKey(a => a.Username);
            builder.HasOne(a => a.Account).WithOne(a => a.Cart).HasForeignKey<Cart>(a => a.Username).HasConstraintName("FK_Cart_Account");
        }
    }
}
