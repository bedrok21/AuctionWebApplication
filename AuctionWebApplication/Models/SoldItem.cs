using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuctionWebApplication.Models;

public partial class SoldItem
{
    public int AuctionId { get; set; }

    [Display(Name = "Остаточна ціна")]
    public decimal? FinalPrice { get; set; }

    public string BidderId { get; set; } = null!;

    [Display(Name = "Аукціон")]
    public virtual Auction Auction { get; set; } = null!;

    [Display(Name = "Покупець")]
    public virtual User Bidder { get; set; } = null!;
}
