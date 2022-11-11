using GameHub.Data;
using GameHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameHub.Controllers
{
    public class ShopController : Controller
    {
        // add DB connection dependency object so we can do CRUD
        private readonly ApplicationDbContext _context;

        //constructor runs automatically when calling this controller and uses our DB connection
        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // fetch list of categories to dispay so customer can pick one
            var categories = _context.Categories.OrderBy(c => c.Name).ToList();
            return View(categories);
        }

        public IActionResult ByCategory(string name)
        {
            // store selected Category name in ViewData for display on page heading
            ViewData["Category"] = name;
            //fetch products in the selected category
            var products = _context.Products.Where(p => p.Category.Name == name).ToList();
            return View(products);
        }

        public IActionResult AddToCart(int id)
        {
            //fetch selected product to get the price
            var product = _context.Products.Find(id);
            var price = product.Price;

            //create and save a new CartItm to the DB
            var cartItem = new CartItem
            {
                ProductId = id,
                Quantity = 1,
                Price = (decimal)price,
                CustomerId = GetCartIdentifier()
            };

            _context.Add(cartItem);
            _context.SaveChanges();

            //redirect to the cart page
            return RedirectToAction("Cart");
        }
        private string GetCartIdentifier()
        {
            //is there already a session var with a cart id?
            //we need to use a session var because HTTP is stateless - variables do not perist between HTTP requests
            if(HttpContext.Session.GetString("CartIdentifier") == null)
            {
                //if not, use a GUID to create a unique id and store in a new session var
                HttpContext.Session.SetString("CartIdentifier", Guid.NewGuid().ToString());
            }

            //if we already have a cart id session var, just use this existing value

            return HttpContext.Session.GetString("CartIdentifier");
        }
        public IActionResult Cart()
        {
            //fetch cartItems for display
            var cartItems = _context.CartItems.ToList();
            return View(cartItems);
        }
        public IActionResult Category(string Name)
        {
            // ensure some category name was passed in
            if(Name == null)
            {
                return RedirectToAction("Index");
            }

            // get the Name param from the URL and store in ViewData object so we can display it
            ViewData["Category"] = Name;

            // use the Product model to create a list of produts in memory for display
            var products = new List<Product>();

            //use loop to make some products
            for(var i = 1; i < 6; i++)
            {
                products.Add(new Product { ProductId = i, Name = "Product " + i.ToString(), Price = 10 + i });
            }

            // load the page and pass it the product data to display
            return View(products);
        }
    }
}
