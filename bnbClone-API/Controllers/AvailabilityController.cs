using bnbClone_API.DTOs;
using bnbClone_API.DTOs.PropertyAvailabilityDTOs;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace bnbClone_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvailabilityController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AvailabilityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /api/availability/{propertyId}
        [HttpGet("{propertyId}")]
        //[AllowAnonymous] //if we want to allow public access to availability
        public async Task<IActionResult> GetAvailability(int propertyId)
        {
            var slots = await _unitOfWork.AvailabilityRepo.FindByPropertyIdAsync(propertyId);
            return Ok(slots);
        }

        // POST: /api/availability
        [HttpPost]
        //[Authorize(Roles = "host")] // requires user to be authenticated
        [HttpPost]
        public async Task<IActionResult> AddAvailability(CreateAvailabilityDTO dto)
        {
            var existingSlot = await _unitOfWork.AvailabilityRepo.FindByPropertyAndDateAsync(dto.PropertyId, dto.Date.Date);

            if (existingSlot != null)
            {
                // Update existing
                existingSlot.IsAvailable = dto.IsAvailable;
                existingSlot.BlockedReason = dto.IsAvailable ? "" : (dto.BlockedReason ?? "");
                existingSlot.Price = dto.Price;
                existingSlot.MinNights = dto.MinNights;
            }
            else
            {
                // Create new
                var newSlot = new PropertyAvailability
                {
                    PropertyId = dto.PropertyId,
                    Date = dto.Date.Date,
                    IsAvailable = dto.IsAvailable,
                    BlockedReason = dto.IsAvailable ? "" : (dto.BlockedReason ?? ""),
                    Price = dto.Price,
                    MinNights = dto.MinNights
                };

                await _unitOfWork.AvailabilityRepo.AddAsync(newSlot);
            }

            await _unitOfWork.SaveAsync();
            return Ok(new { message = "Availability saved." });
        }


        // DELETE: /api/availability/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAvailability(int id)
        {
            bool deleted = await _unitOfWork.AvailabilityRepo.DeleteAsync(id);
            if (!deleted) return NotFound("Availability not found or couldn't be deleted.");

            await _unitOfWork.SaveAsync(); // commit the transaction
            return Ok(new { message = "Availability deleted." });
        }

        //Allows host to update price, dates, or flags on a slot: /api/availability/{id}
        //[Authorize(Roles = "host")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAvailability(int id, CreateAvailabilityDTO dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest("Mismatched slot ID.");

                var slot = await _unitOfWork.AvailabilityRepo.GetByIdAsync(id);
                if (slot == null) return NotFound("Slot not found.");

                // Add validation
                if (dto.Price < 0)
                    return BadRequest("Price cannot be negative");

                if (dto.MinNights < 1)
                    return BadRequest("Minimum nights must be at least 1");

                slot.Price = dto.Price;
                slot.MinNights = dto.MinNights;
                slot.Date = dto.Date;
                slot.BlockedReason = dto.IsAvailable ? "" : (dto.BlockedReason ?? "");
                slot.IsAvailable = dto.IsAvailable;

                await _unitOfWork.SaveAsync();
                return Ok(new {message= "Availability updated." });
            }
            catch (DbUpdateException ex)
            {
                // Log the inner exception for more details
                return StatusCode(500, $"Database update failed: {ex.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        //Lists all slots tied to a host’s properties: /api/availability/host/{hostId}
        //[Authorize(Roles = "host,admin")]
        [HttpGet("host/{hostId}")]
        public async Task<IActionResult> GetByHost(int hostId)
        {
            var slots = await _unitOfWork.AvailabilityRepo.GetAvailabilityByHostIdAsync(hostId);

            var response = slots.Select(a => new CreateAvailabilityDTO
            {
                Id = a.Id, 
                PropertyId = a.PropertyId,
                Date = a.Date,
                IsAvailable = a.IsAvailable,
                BlockedReason = a.BlockedReason,
                Price = a.Price,
                MinNights = a.MinNights
            });

            return Ok(response);
        }

        // Admin/Host: list all availability slots : /api/availability
        //[Authorize(Roles = "admin,host")]
        [HttpGet]
        public async Task<IActionResult> GetAllAvailability()
        {
            var slots = await _unitOfWork.AvailabilityRepo.GetAllAsync();

            var response = slots.Select(a => new CreateAvailabilityDTO
            {
                Id = a.Id,
                PropertyId = a.PropertyId,
                Date = a.Date,
                IsAvailable = a.IsAvailable,
                BlockedReason = a.BlockedReason,
                Price = a.Price,
                MinNights = a.MinNights
            });

            return Ok(response);
        }

    }
}
