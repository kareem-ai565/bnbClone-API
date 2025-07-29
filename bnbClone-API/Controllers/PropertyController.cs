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



        [HttpPost("{id}/place-type")]
        public async Task<IActionResult> SetPlaceType(int id, [FromBody] SetPlaceTypeDto dto)
        {
            var property = await _propertyService.GetByIdAsync(id);
            if (property == null)
                return NotFound();

            property.PropertyType = dto.PlaceType;
            var updated = await _propertyService.UpdateAsync(property);

            if (!updated)
                return StatusCode(500, "Failed to update place type");

            return NoContent();
        }

        [HttpPost("{id}/location")]
        public async Task<IActionResult> SetLocation(int id, [FromBody] SetLocationDto dto)
        {
            var property = await _propertyService.GetByIdAsync(id);
            if (property == null)
                return NotFound();

            property.Latitude = dto.Latitude;
            property.Longitude = dto.Longitude;
            property.Address = dto.Address;

            var updated = await _propertyService.UpdateAsync(property);
            if (!updated)
                return StatusCode(500, "Failed to update location");

            return NoContent();
        }

        [HttpPost("{id}/step4")]
        public async Task<IActionResult> UpdateStep4(int id, [FromBody] Step4Dto dto)
        {
            var property = await _propertyService.GetByIdAsync(id);
            if (property == null)
                return NotFound();

            property.MaxGuests = dto.MaxGuests;
            property.Bedrooms = dto.Bedrooms;
            property.Bathrooms = dto.Bathrooms;

            var updated = await _propertyService.UpdateAsync(property);
            if (!updated)
                return StatusCode(500, "Failed to update step 4 data");

            return NoContent();
        }


        [HttpPost("update-step5/{id}")]
        public async Task<IActionResult> UpdateStep5(int id, [FromBody] PropertyStep5Dto dto)
        {
            var success = await _propertyService.UpdateStep5AmenitiesAsync(id, dto.AmenityIds);

            if (!success)
                return NotFound();

            return Ok();
        }

        // ✅ POST: /api/property/{id}/title
        [HttpPost("{id}/title")]
        public async Task<IActionResult> SetTitle(int id, [FromBody] SetTitleDto dto)
        {
            var property = await _propertyService.GetByIdAsync(id);
            if (property == null)
                return NotFound();

            property.Title = dto.Title;
            var updated = await _propertyService.UpdateAsync(property);
            if (!updated)
                return StatusCode(500, "Failed to update title");

            return NoContent();
        }



        [HttpPost("{id}/description")]
        public async Task<IActionResult> SetDescription(int id, [FromBody] SetDescriptionDto dto)
        {
            var property = await _propertyService.GetByIdAsync(id);
            if (property == null)
                return NotFound();

            property.Description = dto.Description;
            var updated = await _propertyService.UpdateAsync(property);

            if (!updated)
                return StatusCode(500, "Failed to update description");

            return NoContent();
        }

        [HttpPost("{id}/booking-setting")]
        public async Task<IActionResult> SetBookingSetting(int id, [FromBody] SetBookingSettingDto dto)
        {
            var property = await _propertyService.GetByIdAsync(id);
            if (property == null)
                return NotFound();

            property.InstantBook = dto.BookingSetting == "instant";

            var updated = await _propertyService.UpdateAsync(property);
            if (!updated)
                return StatusCode(500, "Failed to update booking setting");

            return NoContent();
        }


        // POST: api/Property/{id}/pricing
        [HttpPost("{id}/pricing")]
        public async Task<IActionResult> SetPricing(int id, [FromBody] SetPricingDto dto)
        {
            var property = await _propertyService.GetByIdAsync(id);
            if (property == null)
                return NotFound();

            property.PricePerNight = dto.BasePrice;
            property.ServiceFee = Math.Round(dto.BasePrice * 0.125m, 2); 
            property.CleaningFee = 0;
            property.UpdatedAt = DateTime.UtcNow;

            var updated = await _propertyService.UpdateAsync(property);
            if (!updated)
                return StatusCode(500, "Failed to update pricing");

            return NoContent();
        }

        [HttpPost("{id}/safety-details")]
        public async Task<IActionResult> SetSafetyDetails(int id, [FromBody] SafetyDetailsDto dto)
        {
            var property = await _propertyService.GetByIdAsync(id);
            if (property == null)
                return NotFound();

            property.HasSecurityCamera = dto.HasSecurityCamera;
            property.HasNoiseMonitor = dto.HasNoiseMonitor;
            property.HasWeapons = dto.HasWeapons;

            var updated = await _propertyService.UpdateAsync(property);
            if (!updated)
                return StatusCode(500, "Failed to update safety details");

            return NoContent();
        }


        [HttpPost("{id}/address")]
        public async Task<IActionResult> SetAddress(int id, [FromBody] SetAddressDto dto)
        {
            var property = await _propertyService.GetByIdAsync(id);
            if (property == null)
                return NotFound();

            property.Country = dto.Country;
            property.Address = dto.Address;
            property.City = dto.City;
            property.PostalCode = dto.PostalCode;

            var updated = await _propertyService.UpdateAsync(property);
            if (!updated)
                return StatusCode(500, "Failed to update address");

            return NoContent();
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProperty(int id, UpdatePropertyDto dto)
        {
            var property = await _propertyService.GetByIdAsync(id);
            if (property == null)
                return NotFound();

            if (!string.IsNullOrWhiteSpace(dto.Title))
                property.Title = dto.Title;

            if (!string.IsNullOrWhiteSpace(dto.Description))
                property.Description = dto.Description;

            if (dto.PricePerNight.HasValue)
                property.PricePerNight = dto.PricePerNight.Value;

            if (dto.MaxGuests.HasValue)
                property.MaxGuests = dto.MaxGuests.Value;

            if (dto.Bedrooms.HasValue)
                property.Bedrooms = dto.Bedrooms.Value;

            if (dto.Bathrooms.HasValue)
                property.Bathrooms = dto.Bathrooms.Value;

            if (!string.IsNullOrWhiteSpace(dto.Address))
                property.Address = dto.Address;

            if (!string.IsNullOrWhiteSpace(dto.City))
                property.City = dto.City;

            if (!string.IsNullOrWhiteSpace(dto.Country))
                property.Country = dto.Country;

            if (!string.IsNullOrWhiteSpace(dto.PropertyType))
                property.PropertyType = dto.PropertyType;

            if (dto.HostId.HasValue)
                property.HostId = dto.HostId.Value;

            var updated = await _propertyService.UpdateAsync(property);
            if (!updated)
                return StatusCode(500, "Failed to update property");

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
