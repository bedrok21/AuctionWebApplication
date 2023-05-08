using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuctionWebApplication.Models;

namespace AuctionWebApplication.Controllers
{
    public class SoldItemsController : Controller
    {
        private readonly DbauctionContext _context;

        public SoldItemsController(DbauctionContext context)
        {
            _context = context;
        }

        // GET: SoldItems
        public async Task<IActionResult> Index()
        {
            var dbauctionContext = _context.SoldItems.Include(s => s.Bidder).Include(s => s.Auction);
            return View(await dbauctionContext.ToListAsync());
        }

        // GET: SoldItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SoldItems == null)
            {
                return NotFound();
            }

            var soldItem = await _context.SoldItems
                .Include(s => s.Bidder)
                .FirstOrDefaultAsync(m => m.AuctionId == id);
            if (soldItem == null)
            {
                return NotFound();
            }

            return View(soldItem);
        }

        // GET: SoldItems/Create
        public IActionResult Create()
        {
            ViewData["AuctionId"] = new SelectList(_context.Auctions, "AuctionId", "AuctionId");
            ViewData["BidderId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: SoldItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuctionId,FinalPrice,BidderId")] SoldItem soldItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(soldItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuctionId"] = new SelectList(_context.Auctions, "AuctionId", "AuctionId", soldItem.AuctionId);
            ViewData["BidderId"] = new SelectList(_context.Users, "UserId", "UserId", soldItem.BidderId);
            return View(soldItem);
        }

        // GET: SoldItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SoldItems == null)
            {
                return NotFound();
            }

            var soldItem = await _context.SoldItems.FindAsync(id);
            if (soldItem == null)
            {
                return NotFound();
            }
            ViewData["AuctionId"] = new SelectList(_context.Auctions, "AuctionId", "AuctionId", soldItem.AuctionId);
            ViewData["BidderId"] = new SelectList(_context.Users, "UserId", "UserId", soldItem.BidderId);
            return View(soldItem);
        }

        // POST: SoldItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuctionId,FinalPrice,BidderId")] SoldItem soldItem)
        {
            if (id != soldItem.AuctionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(soldItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SoldItemExists(soldItem.AuctionId))
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
            ViewData["AuctionId"] = new SelectList(_context.Auctions, "AuctionId", "AuctionId", soldItem.AuctionId);
            ViewData["BidderId"] = new SelectList(_context.Users, "UserId", "UserId", soldItem.BidderId);
            return View(soldItem);
        }

        // GET: SoldItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SoldItems == null)
            {
                return NotFound();
            }
            
            var soldItem = await _context.SoldItems
                .Include(s => s.Bidder)
                .FirstOrDefaultAsync(m => m.AuctionId == id);
            if (soldItem == null)
            {
                return NotFound();
            }

            return View(soldItem);
        }

        // POST: SoldItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SoldItems == null)
            {
                return Problem("Entity set 'DbauctionContext.SoldItems'  is null.");
            }
            var soldItem = await _context.SoldItems
                .FirstOrDefaultAsync();

            if (soldItem != null)
            {
                _context.SoldItems.Remove(soldItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SoldItemExists(int id)
        {
          return (_context.SoldItems?.Any(e => e.AuctionId == id)).GetValueOrDefault();
        }
    }
}
