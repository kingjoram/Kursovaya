using Microsoft.AspNetCore.Identity;

namespace WebApp.Models;

// Add profile data for application users by adding properties to the WebAppUser class
public class AppUser : IdentityUser<Guid>
{
    public AppUser()
    {
        Id = Guid.NewGuid();
    }
}

public class AppRole : IdentityRole<Guid>
{
    public AppRole()
    {
        
    }
    
    public AppRole(string roleName) : base(roleName)
    {
    }
}
public class AppRoleClaim : IdentityRoleClaim<Guid>
{
}
public class AppUserClaim : IdentityUserClaim<Guid>
{
}
public class AppUserLogin : IdentityUserLogin<Guid>
{
}
public class AppUserRole : IdentityUserRole<Guid>
{
}
public class AppUserToken : IdentityUserToken<Guid>
{
}

