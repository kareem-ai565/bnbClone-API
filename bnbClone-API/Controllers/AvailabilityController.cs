using bnbClone_API.DTOs;
using bnbClone_API.DTOs.PropertyAvailabilityDTOs;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> AddAvailability(CreateAvailabilityDTO dto)
        {
            var newSlot = new PropertyAvailability
            {
                PropertyId = dto.PropertyId,
                Date = dto.Date.Date, // normalize to date only: yyyy-mm-dd 00:00:00
                IsAvailable = dto.IsAvailable,
                BlockedReason = dto.BlockedReason,
                Price = dto.Price,
                MinNights = dto.MinNights
            };

            await _unitOfWork.AvailabilityRepo.AddAsync(newSlot);
            await _unitOfWork.SaveChangesAsync(); // commit the transaction
            return Ok("Availability added.");
        }

        // DELETE: /api/availability/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAvailability(int id)
        {
            bool deleted = await _unitOfWork.AvailabilityRepo.DeleteAsync(id);
            if (!deleted) return NotFound("Availability not found or couldn't be deleted.");

            await _unitOfWork.SaveChangesAsync(); // commit the transaction
            return Ok("Availability deleted.");
        }

        ////Allows host to update price, dates, or flags on a slot: /api/availability/{id}
        ////[Authorize(Roles = "host")]
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateAvailability(int id, CreateAvailabilityDTO dto)
        //{
        //    var slot = await _unitOfWork.AvailabilityRepo.GetByIdAsync(id);
        //    if (slot == null) return NotFound("Slot not found.");

        //    slot.Price = dto.Price;
        //    slot.MinNights = dto.MinNights;
        //    slot.Date = dto.Date;
        //    slot.BlockedReason = dto.BlockedReason;
        //    slot.IsAvailable = dto.IsAvailable;

        //    await _unitOfWork.SaveChangesAsync();
        //    return Ok("Availability updated.");
        //}

        ////Lists all slots tied to a host’s properties: /api/availability/host/{hostId}
        ////[Authorize(Roles = "host,admin")]
        //public async Task<IActionResult> GetByHost(int hostId)
        //{
        //    var slots = await _unitOfWork.AvailabilityRepo.GetAvailabilityByHostIdAsync(hostId);

        //    var response = slots.Select(a => new CreateAvailabilityDTO
        //    {
        //        PropertyId = a.PropertyId,
        //        Date = a.Date,
        //        IsAvailable = a.IsAvailable,
        //        BlockedReason = a.BlockedReason,
        //        Price = a.Price,
        //        MinNights = a.MinNights
        //    });

        //    return Ok(response);
        //}
        //// Admin/Host: list all availability slots : /api/availability
        ////[Authorize(Roles = "admin,host")]
        //[HttpGet]
        //public async Task<IActionResult> GetAllAvailability()
        //{
        //    var slots = await _unitOfWork.AvailabilityRepo.GetAllAsync();

        //    var response = slots.Select(a => new CreateAvailabilityDTO
        //    {
        //        PropertyId = a.PropertyId,
        //        Date = a.Date,
        //        IsAvailable = a.IsAvailable,
        //        BlockedReason = a.BlockedReason,
        //        Price = a.Price,
        //        MinNights = a.MinNights
        //    });

        //    return Ok(response);
        //}

    }
}
