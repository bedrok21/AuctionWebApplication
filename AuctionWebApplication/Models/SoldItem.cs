using System;
using System.Collections.Generic;

namespace AuctionWebApplication;

public partial class SoldItem
{
    public int AuctionId { get; set; }

    public decimal? FinalPrice { get; set; }

    public int BidderId { get; set; }

    public virtual Auction Auction { get; set; } = null!;

    public virtual User Bidder { get; set; } = null!;
}
