using System;
using System.Collections.Generic;
using AuctionWebApplication.Models;

namespace AuctionWebApplication.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Auction> Auctions { get; set; } = new List<Auction>();

    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();

    public virtual ICollection<SoldItem> SoldItems { get; set; } = new List<SoldItem>();
}
