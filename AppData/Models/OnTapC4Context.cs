using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AppData.Models;

public partial class OnTapC4Context : DbContext
{
    public OnTapC4Context()
    {
    }

    public OnTapC4Context(DbContextOptions<OnTapC4Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<BillDetail> BillDetails { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartDetail> CartDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ADMIN;Database=ASMCNET105;Trusted_Connection=True;TrustServerCertificate=true;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Username);

            entity.Property(e => e.Username).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(10);
        });

        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasIndex(e => e.Username, "IX_Bills_Username");

            entity.Property(e => e.TotalBill).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.UsernameNavigation).WithMany(p => p.Bills).HasForeignKey(d => d.Username);
        });

        modelBuilder.Entity<BillDetail>(entity =>
        {
            entity.HasIndex(e => e.BillId, "IX_BillDetails_BillId");

            entity.HasIndex(e => e.ProductId, "IX_BillDetails_ProductId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ProductPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Bill).WithMany(p => p.BillDetails).HasForeignKey(d => d.BillId);

            entity.HasOne(d => d.Product).WithMany(p => p.BillDetails).HasForeignKey(d => d.ProductId);
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Username);

            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.UsernameNavigation).WithOne(p => p.Cart)
                .HasForeignKey<Cart>(d => d.Username)
                .HasConstraintName("FK_Cart_Account");
        });

        modelBuilder.Entity<CartDetail>(entity =>
        {
            entity.HasIndex(e => e.CartId, "IX_CartDetails_CartID");

            entity.HasIndex(e => e.ProductId, "IX_CartDetails_ProductId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CartId)
                .HasMaxLength(50)
                .HasColumnName("CartID");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartDetails).HasForeignKey(d => d.CartId);

            entity.HasOne(d => d.Product).WithMany(p => p.CartDetails).HasForeignKey(d => d.ProductId);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
