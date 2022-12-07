using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers;

public class BasketController : Controller
{
    private readonly WebDbContext _context;

    public BasketController(WebDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var findFirst = User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = findFirst?.Value;

        Basket basket = new Basket();
        if (userId != null)
        {
            if (Guid.TryParse(userId, out var useridGuid))
            {
                basket = _context.Basket.Include(x => x.Items).ThenInclude(x => x.Prod)
                    .SingleOrDefault(x => x.UserId == useridGuid);
                if (basket == null)
                {
                    basket = new Basket();
                }
            }
        }

        return View(basket);
    }


    // GET: Basket/Add/Id
    public async Task<IActionResult> Add(Guid? id)
    {
        if (id == null || _context.Products == null)
        {
            return RedirectToAction(nameof(Index), nameof(Product));
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return RedirectToAction(nameof(Index), nameof(Product));
        }

        return View(product);
    }

    // POST: Basket/Add
    [HttpPost, ActionName("Add")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddConfirmed(Guid id)
    {
        if (_context.Products == null)
        {
            return Problem("Entity set 'WebAppContext.Products'  is null.");
        }

        var products = await _context.Products.FindAsync(id);
        if (products != null)
        {
            var findFirst = User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = findFirst?.Value;

            if (userId != null)
            {
                if (Guid.TryParse(userId, out var useridGuid))
                {
                    var basket = _context.Basket.Include(x => x.Items).SingleOrDefault(x => x.UserId == useridGuid);
                    if (basket == null)
                    {
                        var newBasket = new Basket() { UserId = useridGuid };
                        var basketItem = new BasketItem() { ProdId = id, Amount = 1, Price = 12 };
                        newBasket.Items.Add(basketItem);
                        _context.Basket.Add(newBasket);
                    }
                    else
                    {
                        var itemWithExistedProd = basket.Items.FirstOrDefault(x=>x.ProdId == id);
                        if (itemWithExistedProd != null)
                        {
                            itemWithExistedProd.Amount++;
                        }
                        else
                        {
                            var basketItem = new BasketItem() { ProdId = id, Amount = 1, Price = 16 };
                            basket.Items.Add(basketItem);
                        }
                    }
                }
            }
            //_context.Products.Remove(products);
        }

        TempData["Msg"] = "Удачно добавлено";
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index), nameof(Product));
    }

    // GET: Product/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var basketItem = await _context.BasketItem
            .FirstOrDefaultAsync(m => m.Id == id);
        if (basketItem == null)
        {
            return NotFound();
        }

        return View(basketItem);
    }

    // POST: Basket/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var basketItem = await _context.BasketItem
            .Include(x => x.Prod)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (basketItem != null)
        {
            _context.BasketItem.Remove(basketItem);
            TempData["Msg"] = $"{basketItem.Prod.Name}: удалено из корзины";
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}

public class AddProductToShopDto
{
    public Guid ProdId { get; set; }
}