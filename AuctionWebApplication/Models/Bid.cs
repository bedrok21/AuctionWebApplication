using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuctionWebApplication;

public partial class Bid
{
    [Display(Name ="Аукціон")]
    public int AuctionId { get; set; }

    public string BidderId { get; set; } = null!;

    [Display(Name = "Ціна")]
    public decimal Price { get; set; }

    public int BidId { get; set; }

    [Display(Name = "Час ставки")]
    public DateTime BidTime { get; set; }

    public virtual ICollection<Auction> Auctions { get; set; } = new List<Auction>();

    [Display(Name = "Контрагент")]
    public virtual User Bidder { get; set; } = null!;
}
