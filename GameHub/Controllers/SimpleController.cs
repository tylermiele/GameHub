using Microsoft.AspNetCore.Mvc;

namespace GameHub.Controllers
{
    public class SimpleController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Message"] = "Today is " + DateTime.Today.ToString();
            //old code - works but not as good because the view name is implied but not stated explicitly
            //return View();

            //new code - better becasue it's much more explicit
            return View("Index");
        }
    }
}
