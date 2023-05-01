using System;
using System.Collections.Generic;

namespace AuctionWebApplication;

public partial class Auction
{
    public int AuctionId { get; set; }

    public int SellerId { get; set; }

    public string AuctionName { get; set; } = null!;

    public string? AuctionDesription { get; set; }

    public decimal StartPrice { get; set; }

    public DateTime EndTime { get; set; }

    public int? BidId { get; set; }

    public virtual Bid? Bid { get; set; }

    public virtual User Seller { get; set; } = null!;

    public virtual SoldItem? SoldItem { get; set; }
}
