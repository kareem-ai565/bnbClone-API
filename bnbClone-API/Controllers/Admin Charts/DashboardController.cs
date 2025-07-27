using bnbClone_API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace bnbClone_API.Controllers.Admin_Charts
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        public DashboardController(ApplicationDbContext db)
        {
            this.db = db;
        }


        [HttpGet("reviewStats")]
        public async Task<IActionResult> GetReviewStats()
        {
            var totalReviews =await  db.Reviews.CountAsync();
            var avgRating = await db.Reviews.AverageAsync(r => r.Rating);
            var reviewsPerMonth =await  db.Reviews
                .GroupBy(r => new { r.CreatedAt.Year, r.CreatedAt.Month })
                .Select(g => new { g.Key.Year, g.Key.Month, Count = g.Count() })
                .ToListAsync();

            return Ok(new { totalReviews, avgRating, reviewsPerMonth });

        }



        [HttpGet("bookingStats")]
        public async Task<IActionResult> GetBookingStats()
        {
            var totalBookings = await db.Bookings.CountAsync();
            var monthlyBookings = await db.Bookings
                .GroupBy(b => new { b.CreatedAt.Year, b.CreatedAt.Month })
                .Select(g => new { g.Key.Year, g.Key.Month, Count = g.Count() })
                .ToListAsync();

            return Ok(new { totalBookings, monthlyBookings });
        }




        [HttpGet("hostingIncome")]
        public async Task<IActionResult> GetIncome()
        {
            var income = await db.Bookings
                .Where(b => b.Status == "Completed")
                .SumAsync(b => b.TotalAmount);

            var monthlyIncome = await db.Bookings
                .Where(b => b.Status == "Completed")
                .GroupBy(b => new { b.CreatedAt.Year, b.CreatedAt.Month })
                .Select(g => new { g.Key.Year, g.Key.Month, Income = g.Sum(b => b.TotalAmount) })
                .ToListAsync();

            return Ok(new { income, monthlyIncome });
        }




    }

}
