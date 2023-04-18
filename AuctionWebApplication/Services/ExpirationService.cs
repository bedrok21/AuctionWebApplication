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
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DbauctionContext>();
                var expiredData = context.Auctions
                                            .Where(m => m.EndTime < DateTime.Now)
                                            .ToList();

                foreach (var data in expiredData)
                {
                    {
                        var last_bid = await context.Bids.FindAsync(data.BidId);
                        if (last_bid != null)
                        {
                            var soldItem = new SoldItem
                            {
                                AuctionId = data.AuctionId,
                                FinalPrice = last_bid.Price,
                                BidderId = last_bid.BidderId
                            };
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
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
