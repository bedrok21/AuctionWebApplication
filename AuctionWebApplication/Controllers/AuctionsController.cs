using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuctionWebApplication;
using AuctionWebApplication.Models;

namespace AuctionWebApplication.Controllers
{
    public class AuctionsController : Controller
    {
        private readonly DbauctionContext _context;

        public AuctionsController(DbauctionContext context)
        {
            _context = context;
        }

        // GET: Auctions
        public async Task<IActionResult> Index()
        {
            var dbauctionContext = _context.Auctions.Include(a => a.Bid).Include(a => a.Seller);
            return View(await dbauctionContext.ToListAsync());
        }

        // GET: Auctions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Auctions == null)
            {
                return NotFound();
            }

            var auction = await _context.Auctions
                .Include(a => a.Bid)
                .Include(a => a.Seller)
                .FirstOrDefaultAsync(m => m.AuctionId == id);
            if (auction == null)
            {
                return NotFound();
            }

            return View(auction);
        }

        // GET: Auctions/Create
        public IActionResult Create()
        {
            ViewData["BidId"] = new SelectList(_context.Bids, "BidId", "BidId");
            ViewData["SellerId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Auctions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuctionId,SellerId,AuctionName,AuctionDesription,StartPrice,EndTime,BidId")] Auction auction)
        {
            _context.Add(auction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            if (ModelState.IsValid)
            {
                Console.WriteLine("!!!!!!!!!!!");
                _context.Add(auction);
                Console.WriteLine("11111111111");
                await _context.SaveChangesAsync();
                Console.WriteLine("22222222222");
                return RedirectToAction(nameof(Index));
            }
            ViewData["BidId"] = new SelectList(_context.Bids, "BidId", "BidId", auction.BidId);
            ViewData["SellerId"] = new SelectList(_context.Users, "UserId", "UserId", auction.SellerId);
            return View(auction);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Auctions == null)
            {
                return NotFound();
            }

            var auction = await _context.Auctions.FindAsync(id);
            if (auction == null)
            {
                return NotFound();
            }
            ViewData["BidId"] = new SelectList(_context.Bids, "BidId", "BidId", auction.BidId);
            ViewData["SellerId"] = new SelectList(_context.Users, "UserId", "UserId", auction.SellerId);
            return View(auction);
        }

       
        

        // POST: Auctions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuctionId,SellerId,AuctionName,AuctionDesription,StartPrice,EndTime,BidId")] Auction auction)
        {
            if (id != auction.AuctionId)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(auction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuctionExists(auction.AuctionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BidId"] = new SelectList(_context.Bids, "BidId", "BidId", auction.BidId);
            ViewData["SellerId"] = new SelectList(_context.Users, "UserId", "UserId", auction.SellerId);
            return View(auction);
        }

        [HttpPost]
        public async Task<IActionResult> Details(int id, int bet)
        {
            var auction = await _context.Auctions.FindAsync(id);
            if (auction == null)
            {
                return NotFound();
            }
            var currentBid = await _context.Bids.FindAsync(auction.BidId);
            if (currentBid == null && bet < auction.StartPrice)
            {
                TempData["ErrorMessage"] += "Bet must be greater the start price" + "<br>";
            }
            else if (currentBid==null || bet > currentBid.Price )
            {
                var bid = new Bid
                {
                    AuctionId = id,
                    BidderId = 1,
                    Price = bet,
                    BidTime = DateTime.Now
                };
                _context.Bids.Add(bid);
                _context.SaveChanges();
                auction.BidId = bid.BidId;
                _context.Update(auction);
                _context.SaveChanges();
            }
            else
            {
               TempData["ErrorMessage"] += "Bet must be greater the current bid" + "<br>";
            }
            return RedirectToAction(nameof(Details));
    }
       
    
        // GET: Auctions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Auctions == null)
            {
                return NotFound();
            }

            var auction = await _context.Auctions
                .Include(a => a.Bid)
                .Include(a => a.Seller)
                .FirstOrDefaultAsync(m => m.AuctionId == id);
            if (auction == null)
            {
                return NotFound();
            }

            return View(auction);
        }

        // POST: Auctions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Auctions == null)
            {
                return Problem("Entity set 'DbauctionContext.Auctions'  is null.");
            }
            var auction = await _context.Auctions.FindAsync(id);
            if (auction != null)
            {
                _context.Auctions.Remove(auction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuctionExists(int id)
        {
          return (_context.Auctions?.Any(e => e.AuctionId == id)).GetValueOrDefault();
        }
    }
}
