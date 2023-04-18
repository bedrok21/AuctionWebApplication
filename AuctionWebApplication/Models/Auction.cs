using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AuctionWebApplication.Models;

namespace AuctionWebApplication.Models;

public partial class Auction
{
    public int AuctionId { get; set; }

    public int SellerId { get; set; }

    [Display(Name = "Назва")]
    public string AuctionName { get; set; } = null!;

    [Display(Name = "Опис")]
    public string? AuctionDesription { get; set; }

    [Display(Name = "Початкова ціна")]
    public decimal StartPrice { get; set; }

    [Display(Name = "Дата закінчення")]
    public DateTime EndTime { get; set; }
    
    public int? BidId { get; set; }

    public virtual Bid? Bid { get; set; }

    [Display(Name = "Продавець")]
    public virtual User Seller { get; set; } = null!;
}
