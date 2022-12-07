using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers;

[Authorize]
public class ShopController : Controller
{
    private readonly WebDbContext _context;

    public ShopController(WebDbContext context)
    {
        _context = context;
    }

    // GET: Shop
    public async Task<IActionResult> Index()
    {
        return _context.Shop != null ? 
            View(await _context.Shop.ToListAsync()) :
            Problem("Entity set 'WebAppContext.Shop'  is null.");
    }

    // GET: Shop/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null || _context.Shop == null)
        {
            return NotFound();
        }

        var shop = await _context.Shop
            .FirstOrDefaultAsync(m => m.Id == id);
        if (shop == null)
        {
            return NotFound();
        }

        return View(shop);
    }

    // GET: Shop/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Shop/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,PhoneNumber,Adress")] Shop shop)
    {
        if (ModelState.IsValid)
        {
            shop.Id = Guid.NewGuid();
            _context.Add(shop);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(shop);
    }

    // GET: Shop/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null || _context.Shop == null)
        {
            return NotFound();
        }

        var shop = await _context.Shop.FindAsync(id);
        if (shop == null)
        {
            return NotFound();
        }
        return View(shop);
    }

    // POST: Shop/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Id,PhoneNumber,Adress")] Shop shop)
    {
        if (id != shop.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(shop);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShopExists(shop.Id))
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
        return View(shop);
    }

    // GET: Shop/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null || _context.Shop == null)
        {
            return NotFound();
        }

        var shop = await _context.Shop
            .FirstOrDefaultAsync(m => m.Id == id);
        if (shop == null)
        {
            return NotFound();
        }

        return View(shop);
    }

    // POST: Shop/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (_context.Shop == null)
        {
            return Problem("Entity set 'WebAppContext.Shop'  is null.");
        }
        var shop = await _context.Shop.FindAsync(id);
        if (shop != null)
        {
            _context.Shop.Remove(shop);
        }
            
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ShopExists(Guid id)
    {
        return (_context.Shop?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}