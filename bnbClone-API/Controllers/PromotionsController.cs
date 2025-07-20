using bnbClone_API.Models;
using bnbClone_API.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<IEnumerable<Promotion>>> GetActivePromotions()
        {
            var promos = await _unitOfWork.Promotions.GetActivePromotionsAsync();
            return Ok(promos);
        }

        // POST: /api/promotions
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Promotion>> CreatePromotion([FromBody] Promotion promotion)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _unitOfWork.Promotions.AddAsync(promotion);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetActivePromotions), new { id = promotion.Id }, promotion);
        }

        // PUT: /api/promotions/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePromotion(int id, [FromBody] Promotion updatedPromotion)
        {
            var promo = await _unitOfWork.Promotions.GetByIdAsync(id);
            if (promo == null) return NotFound();

            // update fields
            promo.Code = updatedPromotion.Code;
            promo.DiscountType = updatedPromotion.DiscountType;
            promo.Amount = updatedPromotion.Amount;
            promo.StartDate = updatedPromotion.StartDate;
            promo.EndDate = updatedPromotion.EndDate;
            promo.MaxUses = updatedPromotion.MaxUses;
            promo.IsActive = updatedPromotion.IsActive;

            await _unitOfWork.Promotions.UpdateAsync(promo);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        // DELETE: /api/promotions/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePromotion(int id)
        {
            var promo = await _unitOfWork.Promotions.GetByIdAsync(id);
            if (promo == null) return NotFound();

            await _unitOfWork.Promotions.DeleteAsync(id);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }

}
