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
    public class Account_Config : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(p => p.Username);
            builder.Property(p => p.Username).HasColumnType("nvarchar(50)");
            builder.Property(p => p.Password).HasColumnType("nvarchar(50)");
            builder.Property(p => p.Phone).HasColumnType("nvarchar(10)");

        }
    }
}
