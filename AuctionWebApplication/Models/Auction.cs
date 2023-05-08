using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AuctionWebApplication.Models;

namespace AuctionWebApplication;

public partial class Auction
{
    public int AuctionId { get; set; }

    public string SellerId { get; set; } = null!;

    [Display(Name = "Назва")]
    public string AuctionName { get; set; } = null!;

    [Display(Name = "Опис")]
    public string? AuctionDesription { get; set; }

    [Display(Name = "Початкова ціна")]
    public decimal StartPrice { get; set; }

    [Display(Name = "Час завершення")]
    public DateTime EndTime { get; set; }

    public int? BidId { get; set; }

    [Display(Name = "Покупець")]
    public virtual Bid? Bid { get; set; }

    [Display(Name = "Продавець")]
    public virtual User Seller { get; set; } = null!;

    public virtual SoldItem? SoldItem { get; set; }
}
