using System;
using System.Collections.Generic;
using AuctionWebApplication.Models;

namespace AuctionWebApplication;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public decimal Balance { get; set; }

    public decimal Freeze { get; set; }

    public virtual ICollection<Auction> Auctions { get; set; } = new List<Auction>();

    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();

    public virtual ICollection<SoldItem> SoldItems { get; set; } = new List<SoldItem>();
}
