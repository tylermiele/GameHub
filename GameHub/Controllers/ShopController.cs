using Microsoft.AspNetCore.Mvc;

namespace GameHub.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
