using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AuctionWebApplication.Models;

namespace AuctionWebApplication.Models;

public partial class Bid
{
    public int AuctionId { get; set; }

    public int BidderId { get; set; }

    [Display(Name = "Поточна ціна")]
    public decimal Price { get; set; }
    
    public int BidId { get; set; }

    [Display(Name = "Час останньої ставки")]
    public DateTime BidTime { get; set; }

    public virtual ICollection<Auction> Auctions { get; set; } = new List<Auction>();

    public virtual User Bidder { get; set; } = null!;
}
