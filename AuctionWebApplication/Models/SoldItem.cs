using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AuctionWebApplication.Models;

namespace AuctionWebApplication.Models;

public partial class SoldItem
{
    public int AuctionId { get; set; }

    [Display(Name = "Остаточна ціна")]
    public decimal? FinalPrice { get; set; }

    public int BidderId { get; set; }

    public virtual User Bidder { get; set; } = null!;
}
