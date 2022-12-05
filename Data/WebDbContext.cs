using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data;

public class WebDbContext : IdentityDbContext<AppUser, AppRole, Guid,
    AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
{
    public WebDbContext (DbContextOptions<WebDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<Basket> Basket { get; set; } = default!;
    public DbSet<BasketItem> BasketItem { get; set; } = default!;
    public DbSet<Shop> Shop { get; set; } = default!;

    public DbSet<AppUser> Users { get; set; } = default!;
}