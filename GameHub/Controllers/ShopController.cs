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
