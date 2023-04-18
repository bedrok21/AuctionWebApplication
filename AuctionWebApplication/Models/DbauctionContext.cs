using System;
using System.Collections.Generic;
using AuctionWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionWebApplication;

public partial class DbauctionContext : DbContext
{
    public DbauctionContext()
    {
    }

    public DbauctionContext(DbContextOptions<DbauctionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Auction> Auctions { get; set; }

    public virtual DbSet<Bid> Bids { get; set; }

    public virtual DbSet<SoldItem> SoldItems { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server= DESKTOP-KH15CQR\\SQLEXPRESS;Database=DBAuction; Trusted_Connection=True; Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Auction>(entity =>
        {
            entity.ToTable("Auction");

            entity.Property(e => e.AuctionId).HasColumnName("auction_id");
            entity.Property(e => e.AuctionDesription)
                .HasColumnType("text")
                .HasColumnName("auction_desription");
            entity.Property(e => e.AuctionName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("auction_name");
            entity.Property(e => e.BidId).HasColumnName("bid_id");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.SellerId).HasColumnName("seller_id");
            entity.Property(e => e.StartPrice)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("start_price");

            entity.HasOne(d => d.Bid).WithMany(p => p.Auctions)
                .HasForeignKey(d => d.BidId)
                .HasConstraintName("FK_Auction_Bid");

            entity.HasOne(d => d.Seller).WithMany(p => p.Auctions)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Auction_User");
        });

        modelBuilder.Entity<Bid>(entity =>
        {
            entity.ToTable("Bid");

            entity.Property(e => e.BidId).HasColumnName("bid_id");
            entity.Property(e => e.AuctionId).HasColumnName("auction_id");
            entity.Property(e => e.BidTime)
                .HasColumnType("datetime")
                .HasColumnName("bid_time");
            entity.Property(e => e.BidderId).HasColumnName("bidder_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("price");

            entity.HasOne(d => d.Bidder).WithMany(p => p.Bids)
                .HasForeignKey(d => d.BidderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bid_User");
        });

        modelBuilder.Entity<SoldItem>(entity =>
        {
            entity.HasKey(e => e.AuctionId);

            entity.ToTable("SoldItem");

            entity.Property(e => e.AuctionId)
                .ValueGeneratedNever()
                .HasColumnName("auction_id");
            entity.Property(e => e.BidderId).HasColumnName("bidder_id");
            entity.Property(e => e.FinalPrice)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("final_price");

            entity.HasOne(d => d.Bidder).WithMany(p => p.SoldItems)
                .HasForeignKey(d => d.BidderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SoldItem_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
