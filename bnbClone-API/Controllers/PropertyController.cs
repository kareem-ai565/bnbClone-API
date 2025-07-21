using AutoMapper;
using bnbClone_API.DTOs;
using bnbClone_API.DTOs.PropertyDtos;
using bnbClone_API.Models;
using bnbClone_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bnbClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;
        private readonly IMapper _mapper;

        public PropertyController(IPropertyService propertyService, IMapper mapper)
        {
            _propertyService = propertyService;
            _mapper = mapper;
        }

        // GET: api/Property
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyDetailsDto>>> GetAll()
        {
            var properties = await _propertyService.GetAllAsync();
            var result = _mapper.Map<IEnumerable<PropertyDetailsDto>>(properties);
            return Ok(result);
        }

        // GET: api/Property/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyDetailsDto>> GetById(int id)
        {
            var property = await _propertyService.GetByIdAsync(id);
            if (property == null)
                return NotFound();

            var result = _mapper.Map<PropertyDetailsDto>(property);
            return Ok(result);
        }

        // POST: api/Property
        [HttpPost]
        public async Task<ActionResult<PropertyDetailsDto>> Create([FromBody] CreatePropertyDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var property = _mapper.Map<Property>(dto);
            var created = await _propertyService.AddAsync(property);
            var result = _mapper.Map<PropertyDetailsDto>(created);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // PUT: api/Property/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePropertyDto dto)
        {
            var existing = await _propertyService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            _mapper.Map(dto, existing);
            var updated = await _propertyService.UpdateAsync(existing);

            if (!updated)
                return StatusCode(500, "Update failed");

            return NoContent();
        }

        // DELETE: api/Property/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _propertyService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] PropertySearchDto dto)
        {
            var result = await _propertyService.SearchAsync(dto);
            var mapped = _mapper.Map<List<PropertyDetailsDto>>(result);
            return Ok(mapped);
        }


    }
}
