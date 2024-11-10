using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;
using ShoppingCart.Services;

namespace ShoppingCart.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CartService _cartService;
        private readonly UserResolverService _userResolver;

        public ItemsController(ApplicationDbContext context, CartService cartService, UserResolverService userResolver)
        {
            _context = context;
            _cartService = cartService;
            _userResolver = userResolver;
        }

        // GET: Items
        public async Task<IActionResult> Index(string Category, string searchString)
        {
            if (_context.Item == null)
            {
                return Problem("Entity set 'ItemContext.Item'  is null.");
            }

            // Use LINQ to get list of genres.
            IQueryable<string> categoryQuery = from i in _context.Item
                                               orderby i.Category
                                               select i.Category;
            var items = from i in _context.Item
                        select i;

            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Name!.ToUpper().Contains(searchString.ToUpper()));
            }

            if (!string.IsNullOrEmpty(Category))
            {
                items = items.Where(x => x.Category == Category);
            }

            var searchVM = new ItemSearchView
            {
                Categories = new SelectList(await categoryQuery.Distinct().ToListAsync()),
                Items = await items.ToListAsync()
            };

            return View(searchVM);

        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
        // In ItemsController.cs

        // GET: Items/UserHome
        [Authorize]
        // GET: Items/UserHomePage
        [Authorize]
        public async Task<IActionResult> UserHomePage()
        {
            var currentUser = await _userResolver.GetUser();

            // Get the current logged-in user
            var userId = currentUser.Id;

            // Fetch all the items the current user has for sale
            var itemsForSale = await _context.Item
                .Where(i => i.CreatedBy == userId)
                .ToListAsync();

            // Fetch all purchases where the current user is the seller
            var purchases = await _context.Purchase
                .Where(p => p.SellerId == userId)
                .Include(p => p.Buyer)  // Include buyer information
                .ToListAsync();

            // Group purchases by buyer and calculate the total amount spent by each buyer
            var customerPurchases = purchases
                .GroupBy(p => p.Buyer)
                .Select(g => new CustomerReport
                {
                    CustomerName = g.Key.UserName,
                    TotalSpent = g.Sum(p => p.Item.Price * p.Quantity)
                })
                .ToList();

            // Pass both the items for sale and customer purchase report to the view
            var model = new UserHomePageViewModel
            {
                ItemsForSale = itemsForSale,
                CustomersReport = customerPurchases
            };

            return View(model);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminPage(string Category, string searchString)
        {
            if (_context.Item == null)
            {
                return Problem("Entity set 'ItemContext.Item'  is null.");
            }

            // Use LINQ to get list of genres.
            IQueryable<string> categoryQuery = from i in _context.Item
                                               orderby i.Category
                                               select i.Category;
            var items = from i in _context.Item
                        select i;

            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Name!.ToUpper().Contains(searchString.ToUpper()));
            }

            if (!string.IsNullOrEmpty(Category))
            {
                items = items.Where(x => x.Category == Category);
            }

            var searchVM = new AdminSearchView
            {
                Categories = new SelectList(await categoryQuery.Distinct().ToListAsync()),
                Items = await items.ToListAsync()
            };

            return View(searchVM);

        }
        // In your ItemsController or a specific AdminController
        //public IActionResult AdminPage()
        //{
        //    var model = new ItemSearchView
        //    {
        //        Categories = new SelectList(_dbContext.Categories, "Id", "Name"),
        //        Items = _dbContext.Items.ToList() // Fetch the items from your database
        //    };

        //    return View(model);
        //}


        // GET: Items/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,Category,Quantity")] Item item)
        {
            var currentUser = await _userResolver.GetUser();

            // Set the CreatedBy field to the current user's ID (or username, based on your preference)
            if (currentUser != null)
            {
                item.CreatedBy = currentUser.Id; // Assuming you're using the user ID
            }
            else
            {
                return Unauthorized(); // Handle the case where the user is not logged in
            }


            //item.CreatedBy = _userResolver.GetUser(); // Set the user who created the item
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description, Category,Quantity")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
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
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Item.FindAsync(id);
            if (item != null)
            {
                _context.Item.Remove(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Items/Purchase/5
        public async Task<IActionResult> Purchase(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            if (item.Quantity > 0)
            {
                _cartService.AddItem(item, 1);
                item.Quantity--;
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("Index");
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.Id == id);
        }


    }
}
