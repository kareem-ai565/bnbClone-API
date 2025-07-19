using Microsoft.AspNetCore.Mvc;
using bnbClone_API.UnitOfWork;
using bnbClone_API.Services.Interfaces;

namespace bnbClone_API.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService  bookingService)
        {
            _bookingService = bookingService;
        }
        public IActionResult Index()
        {

            return View();
        }
    }
}
