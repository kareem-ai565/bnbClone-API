using bnbClone_API.DTOs.ViolationsDTOs;
using bnbClone_API.Models;
using bnbClone_API.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace bnbClone_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ViolationsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ViolationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Create a new violation report: /api/violations
        //[Authorize(Roles = "guest")]
        [HttpPost]
        public async Task<IActionResult> CreateViolation(CreateViolationDTO dto)
        {
            var violation = new Violation
            {
                ReportedById = dto.ReportedById,
                ReportedHostId = dto.ReportedHostId,
                ReportedPropertyId = dto.ReportedPropertyId,
                ViolationType = dto.ViolationType.ToString(),
                Description = dto.Description,
                Status = ViolationStatus.Pending.ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.ViolationRepo.AddAsync(violation);
            await _unitOfWork.SaveAsync();
            return Ok("Violation report submitted successfully.");
        }

        // Get all violations with full details: /api/violations
        //[Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllViolations()
        {
            var violations = await _unitOfWork.ViolationRepo.GetViolationsWithDetailsAsync();

            var response = violations.Select(v => new ViolationDetailsDTO
            {
                Id = v.Id,
                ViolationType = v.ViolationType,
                Status = v.Status,
                Description = v.Description,
                CreatedAt = v.CreatedAt,
                UpdatedAt = v.UpdatedAt,
                ResolvedAt = v.ResolvedAt,
                AdminNotes = v.AdminNotes,
                Reporter = new ReportedUserDTO
                {
                    Id = v.ReportedBy.Id,
                    FullName = $"{v.ReportedBy.FirstName} {v.ReportedBy.LastName}",
                    ProfilePictureUrl = v.ReportedBy.ProfilePictureUrl ?? "",
                    Role = v.ReportedBy.Role,
                    EmailVerified = v.ReportedBy.EmailVerified
                },
                Host = v.ReportedHost != null ? new ReportedHostDTO
                {
                    Id = v.ReportedHost.Id,
                    FullName = $"{v.ReportedHost.User.FirstName} {v.ReportedHost.User.LastName}",
                    LivesIn = v.ReportedHost.LivesIn,
                    IsVerified = v.ReportedHost.IsVerified,
                    Rating = v.ReportedHost.Rating,
                    StripeAccountId = v.ReportedHost.StripeAccountId
                } : null,
                Property = v.ReportedProperty != null ? new ReportedPropertyDTO
                {
                    Id = v.ReportedProperty.Id,
                    Title = v.ReportedProperty.Title,
                    City = v.ReportedProperty.City,
                    Country = v.ReportedProperty.Country,
                    Status = v.ReportedProperty.Status,
                    PrimaryImage = v.ReportedProperty.PropertyImages.FirstOrDefault()?.ImageUrl ?? ""
                } : null
            });

            return Ok(response);
        }

        // Get a specific violation by ID: /api/violations/{id}
        //[Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetViolationById(int id)
        {
            var v = await _unitOfWork.ViolationRepo.GetViolationByIdWithDetailsAsync(id);
            if (v == null)
                return NotFound("Violation not found.");

            var response = new ViolationDetailsDTO
            {
                Id = v.Id,
                ViolationType = v.ViolationType,
                Status = v.Status,
                Description = v.Description,
                CreatedAt = v.CreatedAt,
                UpdatedAt = v.UpdatedAt,
                ResolvedAt = v.ResolvedAt,
                AdminNotes = v.AdminNotes,
                Reporter = new ReportedUserDTO
                {
                    Id = v.ReportedBy.Id,
                    FullName = $"{v.ReportedBy.FirstName} {v.ReportedBy.LastName}",
                    ProfilePictureUrl = v.ReportedBy.ProfilePictureUrl ?? "",
                    Role = v.ReportedBy.Role,
                    EmailVerified = v.ReportedBy.EmailVerified
                },
                Host = v.ReportedHost != null ? new ReportedHostDTO
                {
                    Id = v.ReportedHost.Id,
                    FullName = $"{v.ReportedHost.User.FirstName} {v.ReportedHost.User.LastName}",
                    LivesIn = v.ReportedHost.LivesIn,
                    IsVerified = v.ReportedHost.IsVerified,
                    Rating = v.ReportedHost.Rating,
                    StripeAccountId = v.ReportedHost.StripeAccountId
                } : null,
                Property = v.ReportedProperty != null ? new ReportedPropertyDTO
                {
                    Id = v.ReportedProperty.Id,
                    Title = v.ReportedProperty.Title,
                    City = v.ReportedProperty.City,
                    Country = v.ReportedProperty.Country,
                    Status = v.ReportedProperty.Status,
                    PrimaryImage = v.ReportedProperty.PropertyImages.FirstOrDefault()?.ImageUrl ?? ""
                } : null
            };

            return Ok(response);
        }

        // Update violation status: /api/violations/{id}/status 
        //[Authorize(Roles = "admin")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string newStatus) // {Pending, UnderReview, Resolved, Dismissed}
        {
            var violation = await _unitOfWork.ViolationRepo.GetByIdAsync(id);
            if (violation == null)
                return NotFound("Violation not found.");

            violation.Status = newStatus;
            violation.UpdatedAt = DateTime.UtcNow;
            violation.ResolvedAt = newStatus == "Resolved" ? DateTime.UtcNow : null;

            await _unitOfWork.SaveAsync();
            return Ok($"Violation status updated to {newStatus}.");
        }

        // Get violations by reporting user: /api/violations/user/{userId}
        //[Authorize(Roles = "guest")]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var results = await _unitOfWork.ViolationRepo.GetViolationsByReporterAsync(userId);
            return Ok(results);
        }

        // Get violations by property: /api/violations/property/{propertyId}
        //[Authorize(Roles = "admin")]
        [HttpGet("property/{propertyId}")]
        public async Task<IActionResult> GetByProperty(int propertyId)
        {
            var results = await _unitOfWork.ViolationRepo.GetViolationsByPropertyAsync(propertyId);
            return Ok(results);
        }

        // Admin deletes a violation : /api/violations/{id}
        //[Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteViolation(int id)
        {
            var violation = await _unitOfWork.ViolationRepo.GetByIdAsync(id);
            if (violation == null)
                return NotFound("Violation not found.");

            await _unitOfWork.ViolationRepo.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
            return Ok("Violation deleted successfully.");
        }

        // Admin edits a violation (type, description, notes) : /api/violations/{id}
        //[Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditViolation(int id, EditViolationDTO dto)
        {
            var violation = await _unitOfWork.ViolationRepo.GetByIdAsync(id);
            if (violation == null)
                return NotFound("Violation not found.");

            violation.ViolationType = dto.ViolationType.ToString();
            violation.Description = dto.Description;
            violation.AdminNotes = dto.AdminNotes;
            violation.Status = dto.Status;
            violation.UpdatedAt = DateTime.UtcNow;
            violation.ResolvedAt = dto.Status == "Resolved" ? DateTime.UtcNow : null;

            await _unitOfWork.SaveAsync();
            return Ok("Violation updated.");
        }

        // List only violations with status = "Pending" : /api/violations/pending
        //[Authorize(Roles = "admin")]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingViolations()
        {
            var violations = await _unitOfWork.ViolationRepo.GetViolationsByStatusAsync("Pending");
            return Ok(violations);
        }

    }
}
