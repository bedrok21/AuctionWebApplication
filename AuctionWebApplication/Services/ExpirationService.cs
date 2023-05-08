using AuctionWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionWebApplication.Services
{
    public class ExpirationService : IHostedService
    {
        private Timer _timer;

        private readonly IServiceProvider _serviceProvider;

        public ExpirationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DbauctionContext>();
                var expiredData = context.Auctions
                                            .Where(m => m.EndTime < DateTime.Now)
                                            .ToList();

                foreach (var data in expiredData)
                {
                    
                    var last_bid = context.Bids.Find(data.BidId);
                    if (last_bid != null)
                    {
                        var buyer = context.Users.Where(c => c.UserId == last_bid.BidderId).FirstOrDefault();
                        var seller = context.Users.Where(c => c.UserId == data.SellerId).FirstOrDefault();
                        buyer.Freeze -= last_bid.Price;
                        buyer.Balance -= last_bid.Price;
                        seller.Balance += last_bid.Price;
                        var soldItem = new SoldItem
                        {
                            AuctionId = data.AuctionId,
                            FinalPrice = last_bid.Price,
                            BidderId = last_bid.BidderId
                        };
                        context.Update(buyer);
                        context.Update(seller);
                        context.SoldItems.Add(soldItem);
                        context.SaveChanges();
                    }
                    var bids = context.Bids
                                    .Where(m => m.AuctionId == data.AuctionId)
                                    .ToList();

                    foreach (var bid in bids)
                    {
                        context.Bids.Remove(bid);
                    }
                    context.SaveChanges();
                    
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
