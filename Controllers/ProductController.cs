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

//[Authorize]
public class ProductController : Controller
{
    private readonly WebDbContext _context;

    public ProductController(WebDbContext context)
    {
        _context = context;
    }

    // GET: Product
    public async Task<IActionResult> Index()
    {
        var incomings = _context.Incoming.Select(x => new{x.ProdId, x.Amount}).ToList();
        var basketItems = _context.BasketItem.Select(x => new{x.ProdId, Amount = -x.Amount}).ToList();
        
        var concat = basketItems.Concat(incomings);
        
        var result = concat.GroupBy(x => x.ProdId).Select(grouping => new
        { 
            grouping.Key, Sum = grouping.Sum(x => x.Amount)
        }).ToDictionary(x => x.Key, x => x.Sum);
        
        var listAsync = await _context.Products.ToListAsync();
        foreach (var product in listAsync)
        {
            if (result.TryGetValue(product.Id, out var sum))
            {
                product.Balance = sum;
            }
        }
        return View(listAsync);
    }

    // GET: Product/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null || _context.Products == null)
        {
            return NotFound();
        }

        var products = await _context.Products
            .FirstOrDefaultAsync(m => m.Id == id);
        if (products == null)
        {
            return NotFound();
        }

        return View(products);
    }

    // GET: Product/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Product/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name, Price")] Product product)
    {
        if (ModelState.IsValid)
        {
            product.Id = Guid.NewGuid();
            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    // GET: Product/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null || _context.Products == null)
        {
            return NotFound();
        }

        var products = await _context.Products.FindAsync(id);
        if (products == null)
        {
            return NotFound();
        }
        return View(products);
    }

    // POST: Product/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Price")] Product product)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsExists(product.Id))
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
        return View(product);
    }

    // GET: Product/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null || _context.Products == null)
        {
            return NotFound();
        }

        var products = await _context.Products
            .FirstOrDefaultAsync(m => m.Id == id);
        if (products == null)
        {
            return NotFound();
        }

        return View(products);
    }

    // POST: Product/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (_context.Products == null)
        {
            return Problem("Entity set 'WebAppContext.Products'  is null.");
        }
        var products = await _context.Products.FindAsync(id);
        if (products != null)
        {
            _context.Products.Remove(products);
        }
            
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProductsExists(Guid id)
    {
        return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    // GET: Product/Incoming
    public async Task<IActionResult> Incoming(Guid? id)
    {
        var incoming = new Incoming();
        incoming.ProdId = id ?? Guid.Empty;
        return View(incoming);
    }
    
    // POST: Product/Incoming
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> IncomingConfirmed(Guid? id, Incoming incoming)
    {
        incoming.Date = DateTime.Now;
        _context.Add(incoming);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    private async Task BalanceCount()
    {
        var incomings = _context.Incoming.Select(x => new{x.ProdId, x.Amount}).ToList();
        var basketItems = _context.BasketItem.Select(x => new{x.ProdId, Amount = -x.Amount}).ToList();

        var concat = basketItems.Concat(incomings);
        
        var result = concat.GroupBy(x => x.ProdId).Select(grouping => new
        { 
            grouping.Key, Sum = grouping.Sum(x => x.Amount)
        }).ToDictionary(x => x.Key, x => x.Sum);

        var listAsync = await _context.Products.ToListAsync();
        foreach (var product in listAsync)
        {
            if (result.TryGetValue(product.Id, out var sum))
            {
                product.Balance = sum;
            }
        }
    }
}
