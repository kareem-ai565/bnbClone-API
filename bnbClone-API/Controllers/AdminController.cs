using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using bnbClone_API.DTOs.AdminDTOs;
using bnbClone_API.DTOs.Admin;
using bnbClone_API.Services.Interfaces;

namespace bnbClone_API.Controllers
{
    [Route("api/admin")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminUserService _adminUserService;
        private readonly IAdminPropertyService _adminPropertyService;
        private readonly IAdminViolationService _adminViolationService;
        private readonly IAdminHostVerificationService _adminHostVerificationService;
        private readonly IAdminNotificationService _adminNotificationService;
        private readonly IAdminDashboardService _adminDashboardService;

        public AdminController(
            IAdminUserService adminUserService,
            IAdminPropertyService adminPropertyService,
            IAdminViolationService adminViolationService,
            IAdminHostVerificationService adminHostVerificationService,
            IAdminNotificationService adminNotificationService,
            IAdminDashboardService adminDashboardService)
        {
            _adminUserService = adminUserService;
            _adminPropertyService = adminPropertyService;
            _adminViolationService = adminViolationService;
            _adminHostVerificationService = adminHostVerificationService;
            _adminNotificationService = adminNotificationService;
            _adminDashboardService = adminDashboardService;
        }

        // User Management Endpoints
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<AdminUserListDto>>> GetAllUsers()
        {
            try
            {
                var users = await _adminUserService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("users/{id}")]
        public async Task<ActionResult<AdminUserResponseDto>> GetUserById(int id)
        {
            try
            {
                var user = await _adminUserService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound($"User with ID {id} not found.");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("users/{id}/ban")]
        public async Task<ActionResult> BanUser(int id, [FromBody] BanUserRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _adminUserService.BanUserAsync(id, request);
                if (!result)
                    return NotFound($"User with ID {id} not found.");

                return Ok(new { message = "User banned successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("users/{id}/unban")]
        public async Task<ActionResult> UnbanUser(int id)
        {
            try
            {
                var result = await _adminUserService.UnbanUserAsync(id);
                if (!result)
                    return NotFound($"User with ID {id} not found.");

                return Ok(new { message = "User unbanned successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("users/{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _adminUserService.DeleteUserAsync(id);
                if (!result)
                    return NotFound($"User with ID {id} not found.");

                return Ok(new { message = "User deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Property Management Endpoints
        [HttpGet("properties")]
        public async Task<ActionResult<IEnumerable<AdminPropertyListDto>>> GetAllProperties()
        {
            try
            {
                var properties = await _adminPropertyService.GetAllPropertiesAsync();
                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("properties/{id}")]
        public async Task<ActionResult<AdminPropertyResponseDto>> GetPropertyById(int id)
        {
            try
            {
                var property = await _adminPropertyService.GetPropertyByIdAsync(id);
                if (property == null)
                    return NotFound($"Property with ID {id} not found.");

                return Ok(property);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("properties/{id}/status")]
        public async Task<ActionResult> UpdatePropertyStatus(int id, [FromBody] PropertyStatusUpdateDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _adminPropertyService.UpdatePropertyStatusAsync(id, request);
                if (!result)
                    return NotFound($"Property with ID {id} not found.");

                return Ok(new { message = "Property status updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("properties/{id}")]
        public async Task<ActionResult> DeleteProperty(int id)
        {
            try
            {
                var result = await _adminPropertyService.DeletePropertyAsync(id);
                if (!result)
                    return NotFound($"Property with ID {id} not found.");

                return Ok(new { message = "Property deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Violation Management Endpoints
        [HttpGet("violations")]
        public async Task<ActionResult<IEnumerable<AdminViolationListDto>>> GetAllViolations()
        {
            try
            {
                var violations = await _adminViolationService.GetAllViolationsAsync();
                return Ok(violations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("violations/{id}")]
        public async Task<ActionResult<AdminViolationResponseDto>> GetViolationById(int id)
        {
            try
            {
                var violation = await _adminViolationService.GetViolationByIdAsync(id);
                if (violation == null)
                    return NotFound($"Violation with ID {id} not found.");

                return Ok(violation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("violations/{id}/status")]
        public async Task<ActionResult> UpdateViolationStatus(int id, [FromBody] ViolationStatusUpdateDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _adminViolationService.UpdateViolationStatusAsync(id, request);
                if (!result)
                    return NotFound($"Violation with ID {id} not found.");

                return Ok(new { message = "Violation status updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Host Verification Endpoints
        [HttpGet("host-verifications")]
        public async Task<ActionResult<IEnumerable<AdminHostVerificationListDto>>> GetAllHostVerifications()
        {
            try
            {
                var verifications = await _adminHostVerificationService.GetAllVerificationsAsync();
                return Ok(verifications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("host-verifications/{id}")]
        public async Task<ActionResult<AdminHostVerificationResponseDto>> GetHostVerificationById(int id)
        {
            try
            {
                var verification = await _adminHostVerificationService.GetVerificationByIdAsync(id);
                if (verification == null)
                    return NotFound($"Host verification with ID {id} not found.");

                return Ok(verification);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("host-verifications/{id}/approve")]
        public async Task<ActionResult> ApproveHostVerification(int id, [FromBody] HostVerificationStatusUpdateDto request)
        {
            try
            {
                request.Status = "approved";
                var result = await _adminHostVerificationService.UpdateVerificationStatusAsync(id, request);
                if (!result)
                    return NotFound($"Host verification with ID {id} not found.");

                return Ok(new { message = "Host verification approved successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("host-verifications/{id}/reject")]
        public async Task<ActionResult> RejectHostVerification(int id, [FromBody] HostVerificationStatusUpdateDto request)
        {
            try
            {
                request.Status = "rejected";
                var result = await _adminHostVerificationService.UpdateVerificationStatusAsync(id, request);
                if (!result)
                    return NotFound($"Host verification with ID {id} not found.");

                return Ok(new { message = "Host verification rejected successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Notification Endpoints
        [HttpPost("notifications")]
        public async Task<ActionResult<AdminNotificationResponseDto>> SendNotification([FromBody] AdminNotificationRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _adminNotificationService.SendNotificationAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Dashboard Endpoints
        [HttpGet("reports/summary")]
        public async Task<ActionResult<AdminDashboardSummaryDto>> GetDashboardSummary()
        {
            try
            {
                var summary = await _adminDashboardService.GetDashboardSummaryAsync();
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}