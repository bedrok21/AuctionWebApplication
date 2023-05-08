﻿// <auto-generated />
using System;
using AuctionWebApplication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AuctionWebApplication.Migrations
{
    [DbContext(typeof(DbauctionContext))]
    [Migration("20230508223634_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AuctionWebApplication.Auction", b =>
                {
                    b.Property<int>("AuctionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("auction_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuctionId"));

                    b.Property<string>("AuctionDesription")
                        .HasColumnType("text")
                        .HasColumnName("auction_desription");

                    b.Property<string>("AuctionName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("auction_name");

                    b.Property<int?>("BidId")
                        .HasColumnType("int")
                        .HasColumnName("bid_id");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime")
                        .HasColumnName("end_time");

                    b.Property<string>("SellerId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("seller_id");

                    b.Property<decimal>("StartPrice")
                        .HasColumnType("decimal(12, 2)")
                        .HasColumnName("start_price");

                    b.HasKey("AuctionId");

                    b.HasIndex("BidId");

                    b.HasIndex("SellerId");

                    b.ToTable("Auction", (string)null);
                });

            modelBuilder.Entity("AuctionWebApplication.Bid", b =>
                {
                    b.Property<int>("BidId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("bid_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BidId"));

                    b.Property<int>("AuctionId")
                        .HasColumnType("int")
                        .HasColumnName("auction_id");

                    b.Property<DateTime>("BidTime")
                        .HasColumnType("datetime")
                        .HasColumnName("bid_time");

                    b.Property<string>("BidderId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("bidder_id");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(12, 2)")
                        .HasColumnName("price");

                    b.HasKey("BidId");

                    b.HasIndex("BidderId");

                    b.ToTable("Bid", (string)null);
                });

            modelBuilder.Entity("AuctionWebApplication.Models.SoldItem", b =>
                {
                    b.Property<int>("AuctionId")
                        .HasColumnType("int")
                        .HasColumnName("auction_id");

                    b.Property<string>("BidderId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("bidder_id");

                    b.Property<decimal?>("FinalPrice")
                        .HasColumnType("decimal(12, 2)")
                        .HasColumnName("final_price");

                    b.HasKey("AuctionId");

                    b.HasIndex("BidderId");

                    b.ToTable("SoldItem", (string)null);
                });

            modelBuilder.Entity("AuctionWebApplication.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("user_id");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(12, 2)")
                        .HasColumnName("balance");

                    b.Property<decimal>("Freeze")
                        .HasColumnType("decimal(12, 2)")
                        .HasColumnName("freeze");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("name");

                    b.HasKey("UserId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("AuctionWebApplication.Auction", b =>
                {
                    b.HasOne("AuctionWebApplication.Bid", "Bid")
                        .WithMany("Auctions")
                        .HasForeignKey("BidId")
                        .HasConstraintName("FK_Auction_Bid");

                    b.HasOne("AuctionWebApplication.User", "Seller")
                        .WithMany("Auctions")
                        .HasForeignKey("SellerId")
                        .IsRequired()
                        .HasConstraintName("FK_Auction_User");

                    b.Navigation("Bid");

                    b.Navigation("Seller");
                });

            modelBuilder.Entity("AuctionWebApplication.Bid", b =>
                {
                    b.HasOne("AuctionWebApplication.User", "Bidder")
                        .WithMany("Bids")
                        .HasForeignKey("BidderId")
                        .IsRequired()
                        .HasConstraintName("FK_Bid_User");

                    b.Navigation("Bidder");
                });

            modelBuilder.Entity("AuctionWebApplication.Models.SoldItem", b =>
                {
                    b.HasOne("AuctionWebApplication.Auction", "Auction")
                        .WithOne("SoldItem")
                        .HasForeignKey("AuctionWebApplication.Models.SoldItem", "AuctionId")
                        .IsRequired()
                        .HasConstraintName("FK_SoldItem_Auction");

                    b.HasOne("AuctionWebApplication.User", "Bidder")
                        .WithMany("SoldItems")
                        .HasForeignKey("BidderId")
                        .IsRequired()
                        .HasConstraintName("FK_SoldItem_User");

                    b.Navigation("Auction");

                    b.Navigation("Bidder");
                });

            modelBuilder.Entity("AuctionWebApplication.Auction", b =>
                {
                    b.Navigation("SoldItem");
                });

            modelBuilder.Entity("AuctionWebApplication.Bid", b =>
                {
                    b.Navigation("Auctions");
                });

            modelBuilder.Entity("AuctionWebApplication.User", b =>
                {
                    b.Navigation("Auctions");

                    b.Navigation("Bids");

                    b.Navigation("SoldItems");
                });
#pragma warning restore 612, 618
        }
    }
}
