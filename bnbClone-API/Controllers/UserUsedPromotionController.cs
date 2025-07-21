using bnbClone_API.DTOs;
using bnbClone_API.Models;
using bnbClone_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace bnbClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserUsedPromotionController : ControllerBase
    {
        private readonly IUserUsedPromotionService userUsedPromotion;

        public UserUsedPromotionController(IUserUsedPromotionService userUsedPromotion)
        {
            this.userUsedPromotion = userUsedPromotion;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUserPromotions()
        {
             return Ok(await userUsedPromotion.GetAllUserPromotions());
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllPromotionsOfUser(int id)
        {
            return Ok(await userUsedPromotion.GetAllPromotionsOfUser(id));
        }


        [HttpPost]
        public async Task<IActionResult> AddUserPromotions(UserUsedPromotionDTO usedPromotion)
        {
            if (usedPromotion != null) {
               await userUsedPromotion.AddUserPromotion(usedPromotion);
                return Ok(usedPromotion);
            }
            else
            {
                return BadRequest(new { error = "U Must Enter Required Data" });
            }

        }



    }
}
