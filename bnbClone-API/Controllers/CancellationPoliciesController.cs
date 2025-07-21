// Controllers/CancellationPoliciesController.cs
using bnbClone_API.DTOs;
using bnbClone_API.DTOs.CancelationPolcyDts;
using bnbClone_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bnbClone_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CancellationPoliciesController : ControllerBase
    {
        private readonly ICancellationPolicyService _service;

        public CancellationPoliciesController(ICancellationPolicyService service)
        {
            _service = service;
        }


        // ✅ GET: api/cancellationpolicies
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        // ✅ POST: api/cancellationpolicies
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CancellationPolicyCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
        }

        // ✅ PUT: api/cancellationpolicies/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CancellationPolicyUpdateDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated ? NoContent() : NotFound();
        }

        // ✅ DELETE: api/cancellationpolicies/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
