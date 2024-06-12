﻿using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace OnTap_Net104.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext()
        {
            
        }
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<BillDetail> BillDetails { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Bill> Bills { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer("Server=ADMIN;Database=APPC5ASSM;Trusted_Connection=True;TrustServerCertificate=true;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
