using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuctionWebApplication.Models;

public partial class Auction
{
    public int AuctionId { get; set; }

    public int SellerId { get; set; }

    [Display(Name = "Назва")]
    [Required]
    public string AuctionName { get; set; } = null!;

    [Display(Name = "Опис")]
    public string? AuctionDesription { get; set; }

    [Display(Name = "Початкова ціна")]
    [Required]
    public decimal StartPrice { get; set; }

    [Display(Name = "Кінець аукціону")]
    [Required]
    public DateTime EndTime { get; set; }

    public int? BidId { get; set; }

    public virtual Bid? Bid { get; set; }

    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();

    public virtual User Seller { get; set; } = null!;
}
