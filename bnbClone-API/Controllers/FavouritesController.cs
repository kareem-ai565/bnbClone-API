using bnbClone_API.DTOs;
using bnbClone_API.DTOs.FavouritesDTOs;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace bnbClone_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class FavouritesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public FavouritesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Add to favourites: /api/favorites/{propertyId}
        [HttpPost("user/{userId}/{propertyId}")]
        public async Task<IActionResult> AddToFavourites(int propertyId, int userId)
        {
            var exists = await _unitOfWork.FavouriteRepo.IsPropertyFavouritedByUserAsync(userId, propertyId);
            if (exists)
                return BadRequest("Property already favourited.");

            var favourite = new Favourite
            {
                UserId = userId,
                PropertyId = propertyId
            };

            await _unitOfWork.FavouriteRepo.AddAsync(favourite);
            await _unitOfWork.SaveAsync(); // Commit transaction
            return Ok("Property added to favourites.");
        }


        // Remove from favourites: /api/favorites/{propertyId}
        [HttpDelete("user/{userId}/{propertyId}")]
        public async Task<IActionResult> RemoveFromFavourites(int userId, int propertyId)
        {
            var favourite = await _unitOfWork.FavouriteRepo.GetFavouriteByUserAndPropertyAsync(userId, propertyId);
            if (favourite == null)
                return NotFound("Favourite not found.");

            await _unitOfWork.FavouriteRepo.DeleteAsync(favourite.Id);
            await _unitOfWork.SaveAsync(); // Commit transaction
            return Ok("Property removed from favourites.");
        }


        // Get all favourites for current user: /api/favorites
        [HttpGet("user/{userId}/favourites")]
        public async Task<IActionResult> GetUserFavourites(int userId)
        {
            var favourites = await _unitOfWork.FavouriteRepo.GetFavouritesByUserIdAsync(userId);

            var response = favourites.Select(f => new FavouritePropertiesDTO
            {
                PropertyId = f.Property.Id,
                Title = f.Property.Title,
                City = f.Property.City,
                Country = f.Property.Country,
                PricePerNight = f.Property.PricePerNight,
                Currency = f.Property.Currency,
                ImageUrl = f.Property.PropertyImages.FirstOrDefault()?.ImageUrl ?? "",
                FavouritedAt = f.FavouritedAt
            });

            return Ok(response);
        }
        //Checks if the current user has favorited a specific property: /api/favorites/check/{propertyId} 
        // to use in the frontend to show favourite button state
        //[Authorize(Roles = "guest")]
        [HttpGet("check/{propertyId}")]
        public async Task<IActionResult> IsFavourited(int propertyId)
        {
             int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            //int userId = 1; // Hardcoded test user ID for now

            var exists = await _unitOfWork.FavouriteRepo.IsPropertyFavouritedByUserAsync(userId, propertyId);
            return Ok(new { isFavourited = exists });
        }

    }
}
