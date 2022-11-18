using GameHub.Data;
using GameHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            //check if this user already has this item in their cart. SingleOrDefault selects only 1 record or nothing
            var cartItem = _context.CartItems.SingleOrDefault(c => c.ProductId == id && c.CustomerId == GetCartIdentifier());
            if(cartItem == null)
            {
                //create and save a new CartItm to the DB
                cartItem = new CartItem
                {
                    ProductId = id,
                    Quantity = 1,
                    Price = (decimal)price,
                    CustomerId = GetCartIdentifier()
                };
                _context.Add(cartItem);

            }
            else
            {
                cartItem.Quantity += 1;
                _context.Update(cartItem);
            }
           

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
            var cartIdentifier = GetCartIdentifier();
            //fetch cartItems for display
            var cartItems = _context.CartItems.Include(c => c.Product).Where(c => c.CustomerId == cartIdentifier).ToList();

            //count items in cart to display in nav
            var itemCount = (from c in cartItems select c.Quantity).Sum();
            HttpContext.Session.SetInt32("ItemCount", itemCount);

            // calculate cart total to display on cart page
            var cartTotal = (from c in cartItems select c.Quantity * c.Price).Sum();
            ViewData["CartTotal"] = cartTotal;

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
        // GET: /Shop/RemoveFromCart/5 => delete thsi itemfrom the current cart
        public IActionResult RemoveFromCart(int id)
        {
            var cartItem = _context.CartItems.SingleOrDefault(c => c.CartItemId == id);
            _context.Remove(cartItem);
            _context.SaveChanges();

            return RedirectToAction("Cart");
        }

        //GET: /Shop/Checkout => show checkout form. only for authenicated users
        [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }

        //POST: /Shop/Checkout => save all order and customer data to a session var
        [HttpPost]
        public IActionResult Checkout([Bind("FirstName,LastName,Address,City,Province,PostalCode,Phone")] Order order)
        {
            //create new order in memory from the form inputs. save to DB only after successful payment.
            //auto-fill some of the values
            order.OrderDate = DateTime.Now;
            order.CustomerId = User.Identity.Name;

            //calculate the order total
            var cartItems = _context.CartItems.Where(c => c.CustomerId == HttpContext.Session.GetString("CartIdentifier"));
            order.OrderTotal = (from c in cartItems select c.Quantity * c.Price).Sum();

            //save the order to a session var
            // .NET session only support int and String so we need to add an extension library
            HttpContext.Session.SetObject("Order", order);

            // start Stripe payment
            return RedirectToAction("Payment");
        }
    }
}
