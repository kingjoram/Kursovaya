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
        return _context.Products != null ? 
            View(await _context.Products.ToListAsync()) :
            Problem("Entity set 'WebAppContext.Products'  is null.");
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
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
}