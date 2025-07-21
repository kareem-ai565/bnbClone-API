using bnbClone_API.DTOs;
using bnbClone_API.DTOs.PromotionDTOs;
using bnbClone_API.Models;
using bnbClone_API.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bnbClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PromotionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /api/promotions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PromotionReadDto>>> GetActivePromotions()
        {
            var promotions = await _unitOfWork.Promotions.GetActivePromotionsAsync();

            var result = promotions.Select(p => new PromotionReadDto
            {
                Id = p.Id,
                Code = p.Code,
                DiscountType = p.DiscountType,
                Amount = p.Amount,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                MaxUses = p.MaxUses,
                UsedCount = p.UsedCount,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt
            });

            return Ok(result);
        }

        // POST: /api/promotions
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreatePromotion(PromotionCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var promotion = new Promotion
            {
                Code = dto.Code,
                DiscountType = dto.DiscountType,
                Amount = dto.Amount,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                MaxUses = dto.MaxUses,
                UsedCount = 0,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Promotions.AddAsync(promotion);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetActivePromotions), new { id = promotion.Id }, promotion);
        }

        // PUT: /api/promotions/{id}
        [HttpPut("{id}")]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePromotion(int id, PromotionUpdateDto dto)
        {
            var promotion = await _unitOfWork.Promotions.GetByIdAsync(id);
            if (promotion == null) return NotFound();

            promotion.Code = dto.Code;
            promotion.DiscountType = dto.DiscountType;
            promotion.Amount = dto.Amount;
            promotion.StartDate = dto.StartDate;
            promotion.EndDate = dto.EndDate;
            promotion.MaxUses = dto.MaxUses;
            promotion.IsActive = dto.IsActive;

            await _unitOfWork.Promotions.UpdateAsync(promotion);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        // DELETE: /api/promotions/{id}
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePromotion(int id)
        {
            var promotion = await _unitOfWork.Promotions.GetByIdAsync(id);
            if (promotion == null) return NotFound();

            await _unitOfWork.Promotions.DeleteAsync(id);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
