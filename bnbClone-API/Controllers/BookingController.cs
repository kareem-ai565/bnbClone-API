using Microsoft.AspNetCore.Mvc;

namespace bnbClone_API.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
