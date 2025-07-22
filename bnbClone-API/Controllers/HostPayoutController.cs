using bnbClone_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bnbClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostPayoutController : ControllerBase
    {
        private readonly IHostPayoutService hostPayoutService;

        public HostPayoutController(IHostPayoutService hostPayoutService)
        {
            this.hostPayoutService = hostPayoutService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPayouts()
        {
            var payouts = await hostPayoutService.GetAllAsync();
            return Ok(payouts);
        }
        [HttpGet("by-host/{hostId}")]
        public async Task<IActionResult> GetPayoutsByHostId(int hostId)
        {
            var payouts = await hostPayoutService.GetByHostIdAsync(hostId);
            return Ok(payouts);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayoutById(int id)
        {
            var payout = await hostPayoutService.GetByIdAsync(id);
            if (payout == null)
            {
                return NotFound($"Payout with ID {id} not found.");
            }
            return Ok(payout);
        }
    }
}
