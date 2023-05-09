using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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
            var dbauctionContext = _context.Auctions
                .Include(a => a.Seller)
                .Include(a => a.Bid)
                .Where(a => a.EndTime > DateTime.Now);
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
                .Include(a => a.Seller)
                .Include(a =>a.Bid).Where(a => a.EndTime > DateTime.Now)
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
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                TempData["ErrorMessage"] += "Require Authentication" + "<br>";
                return RedirectToAction("Index", "Auctions");
            }
        }

        // POST: Auctions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuctionId,SellerId,AuctionName,AuctionDesription,StartPrice,EndTime,BidId")] Auction auction)
        {
            auction.SellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _context.Add(auction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuctionId,SellerId,AuctionName,AuctionDesription,StartPrice,EndTime,BidId")] Auction auction)
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

        [HttpPost]
        public async Task<IActionResult> Details(int id, int bet)
        {
            var newUser = _context.Users.Where(c => c.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault();
            var auction = await _context.Auctions.FindAsync(id);
            if (auction == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var currentBid = await _context.Bids.FindAsync(auction.BidId);
            if (newUser== null) 
            {
                TempData["ErrorMessage"] += "Ставити ставку можуть лише авторизовані користувачі" + "<br>";
            }
            else if (auction.SellerId== User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                TempData["ErrorMessage"] += "Не можливо поставити ставку на свій аукціон" + "<br>";
            }
            else if (bet > newUser.Balance - newUser.Freeze)
            {
                TempData["ErrorMessage"] += "Недостатній баланс для здійснення ставки" + "<br>";
            }
            else if (currentBid == null && bet < auction.StartPrice)
            {
                TempData["ErrorMessage"] += "Ставка повинна бути більшою ніж початкова ціна" + "<br>";
            }
            else if (currentBid==null || bet > currentBid.Price )
            {
                if (currentBid != null)
                {
                    var curUser = _context.Users.Where(c => c.UserId == currentBid.BidderId).FirstOrDefault();
                    curUser.Freeze -= currentBid.Price;
                    _context.Update(curUser);
                }
                var bid = new Bid
                {
                    AuctionId = id,
                    BidderId = newUser.UserId,
                    Price = bet,
                    BidTime = DateTime.Now
                };
                
                newUser.Freeze += bet;
                _context.Update(newUser);
                _context.Bids.Add(bid);
                _context.SaveChanges();
                auction.BidId = bid.BidId;
                auction.Bid = bid;
                _context.Update(auction);
                _context.SaveChanges();
            }
            else if (bet <= currentBid.Price)
            {
               TempData["ErrorMessage"] += "Ставка повинна бути більшою ніж остання ставка" + "<br>";
            }
            
            return View(auction);
        }
       
    
        // GET: Auctions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Auctions == null)
            {
                return NotFound();
            }

            var auction = await _context.Auctions
                .Include(a => a.Seller)
                .Include(a => a.Bid).Where(a => a.EndTime > DateTime.Now)
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
