using bnbClone_API.DTOs;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace bnbClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // POST: /api/reviews
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReview([FromBody] ReviewCreateDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var booking = await _unitOfWork.BookingRepo.GetByIdAsync(dto.BookingId);
            if (booking == null || userId == null)
                return BadRequest("Invalid booking or not authorized");

            var review = new Review
            {
                BookingId = dto.BookingId,
                ReviewerId = userId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Reviews.AddAsync(review);
            return Ok(review);
        }

        // GET: /api/reviews/property/{propertyId}
        [HttpGet("property/{propertyId}")]
        public async Task<IActionResult> GetByPropertyId(int propertyId)
        {
            var reviews = await _unitOfWork.Reviews.GetReviewsByPropertyIdAsync(propertyId);
            var result = reviews.Select(r => new ReviewReadDto
            {
                Id = r.Id,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                ReviewerName = $"{r.Reviewer.FirstName} {r.Reviewer.LastName}"
            });

            return Ok(result);
        }

        // GET: /api/reviews/user/{userId}
        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var reviews = await _unitOfWork.Reviews.GetReviewsByUserIdAsync(userId);
            return Ok(reviews);
        }
    }
}
