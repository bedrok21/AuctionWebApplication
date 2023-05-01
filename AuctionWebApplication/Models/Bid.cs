using System;
using System.Collections.Generic;

namespace AuctionWebApplication;

public partial class Bid
{
    public int AuctionId { get; set; }

    public int BidderId { get; set; }

    public decimal Price { get; set; }

    public int BidId { get; set; }

    public DateTime BidTime { get; set; }

    public virtual ICollection<Auction> Auctions { get; set; } = new List<Auction>();

    public virtual User Bidder { get; set; } = null!;
}
