using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ShoppingCart.Models;
using System.Threading.Tasks;

public class UserResolverService
{
    private readonly IHttpContextAccessor _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserResolverService(IHttpContextAccessor context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<ApplicationUser> GetUser()
    {
        return await _userManager.FindByEmailAsync(_context.HttpContext.User?.Identity?.Name);
    }
}
