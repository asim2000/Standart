using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class Home : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
