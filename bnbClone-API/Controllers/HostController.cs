using bnbClone_API.DTOs.HostDTOs;
using bnbClone_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace bnbClone_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Host")]
    public class HostController : ControllerBase
    {
        private readonly IHostService _hostService;

        public HostController(IHostService hostService)
        {
            _hostService = hostService;
        }

        // GET: api/host
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var hosts = await _hostService.GetAllHostsAsync();
            return Ok(hosts);
        }

        // GET: api/host/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var host = await _hostService.GetHostByIdAsync(id);
            if (host == null)
                return NotFound("Host not found");

            return Ok(host);
        }

        // GET: api/host/me
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrent()
        {
            int userId = GetUserId();
            var host = await _hostService.GetHostByUserIdAsync(userId);
            if (host == null)
                return NotFound("Host not found");

            return Ok(host);
        }

        // PUT: api/host/me
        [HttpPut("me")]
        public async Task<IActionResult> UpdateCurrent([FromBody] HostUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int userId = GetUserId();
            var updated = await _hostService.UpdateHostByUserIdAsync(userId, updateDto);

            if (!updated)
                return BadRequest("Update failed");

            return NoContent();
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null
                ? int.Parse(userIdClaim.Value)
                : throw new UnauthorizedAccessException("User ID not found");
        }
    }
}
