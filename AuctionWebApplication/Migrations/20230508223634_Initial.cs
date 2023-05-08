using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    balance = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    freeze = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "Bid",
                columns: table => new
                {
                    bid_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    auction_id = table.Column<int>(type: "int", nullable: false),
                    bidder_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    price = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    bid_time = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bid", x => x.bid_id);
                    table.ForeignKey(
                        name: "FK_Bid_User",
                        column: x => x.bidder_id,
                        principalTable: "User",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Auction",
                columns: table => new
                {
                    auction_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    seller_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    auction_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    auction_desription = table.Column<string>(type: "text", nullable: true),
                    start_price = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    end_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    bid_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auction", x => x.auction_id);
                    table.ForeignKey(
                        name: "FK_Auction_Bid",
                        column: x => x.bid_id,
                        principalTable: "Bid",
                        principalColumn: "bid_id");
                    table.ForeignKey(
                        name: "FK_Auction_User",
                        column: x => x.seller_id,
                        principalTable: "User",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "SoldItem",
                columns: table => new
                {
                    auction_id = table.Column<int>(type: "int", nullable: false),
                    final_price = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    bidder_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldItem", x => x.auction_id);
                    table.ForeignKey(
                        name: "FK_SoldItem_Auction",
                        column: x => x.auction_id,
                        principalTable: "Auction",
                        principalColumn: "auction_id");
                    table.ForeignKey(
                        name: "FK_SoldItem_User",
                        column: x => x.bidder_id,
                        principalTable: "User",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auction_bid_id",
                table: "Auction",
                column: "bid_id");

            migrationBuilder.CreateIndex(
                name: "IX_Auction_seller_id",
                table: "Auction",
                column: "seller_id");

            migrationBuilder.CreateIndex(
                name: "IX_Bid_bidder_id",
                table: "Bid",
                column: "bidder_id");

            migrationBuilder.CreateIndex(
                name: "IX_SoldItem_bidder_id",
                table: "SoldItem",
                column: "bidder_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoldItem");

            migrationBuilder.DropTable(
                name: "Auction");

            migrationBuilder.DropTable(
                name: "Bid");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
