using GameHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameHub.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
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
