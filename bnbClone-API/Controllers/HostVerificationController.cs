using bnbClone_API.DTOs;
using bnbClone_API.Models;
using bnbClone_API.Services.Impelementations;
using bnbClone_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.DTOs;

namespace bnbClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostVerificationController : ControllerBase
    {
        private readonly IhostVerificationService _ihostVerification;

        public HostVerificationController(IhostVerificationService ihostVerification)
        {
            this._ihostVerification = ihostVerification;
        }



        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllVerifications()
        {
            IEnumerable<HostVerification> hostVerifications = await _ihostVerification.GetAllHostVerification();

            if (hostVerifications != null)
            {
                return Ok(hostVerifications);
            }
            else
            {
                return NotFound(new { error = "No Verifications" });
            }

        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetVerificationById(int id)
        {
            try
            {
                var verification = await _ihostVerification.GetHostVerificationById(id);
                return Ok(verification);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpGet("status/{status}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetVerificationsByStatus(string status)
        {
            try
            {
                var verifications = await _ihostVerification.GetHostVerificationsByStatus(status);

                if (verifications.Any())
                {
                    return Ok(verifications);
                }
                else
                {
                    return NotFound(new { error = $"No verifications found with status: {status}" });
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpGet("host/{hostId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetVerificationsByHostId(int hostId)
        {
            try
            {
                var verifications = await _ihostVerification.GetHostVerificationsByHostId(hostId);

                if (verifications.Any())
                {
                    return Ok(verifications);
                }
                else
                {
                    return NotFound(new { error = $"No verifications found for host ID: {hostId}" });
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        //[Consumes("multipart/form-data")]
        //[HttpPost]
        //public async Task<IActionResult> AddVerifications([FromForm] HostVerificationDTO? hostDto = null)
        //{

        //    Console.WriteLine("AddVerifications endpoint hit!");

        //    try
        //    {
        //        Console.WriteLine("AddVerifications endpoint hit!");

        //        if (hostDto == null)
        //            return BadRequest(new { error = "Enter Required Data" });

        //        Console.WriteLine("DTO received:");
        //        Console.WriteLine("Type: " + hostDto.Type);
        //        Console.WriteLine("Doc1: " + hostDto.DocumentUrl1?.FileName);
        //        Console.WriteLine("Doc2: " + hostDto.DocumentUrl2?.FileName);

        //        // Try-catch the service layer too
        //        try
        //        {
        //            await _ihostVerification.AddHostVerification(hostDto);
        //        }
        //        catch (Exception innerEx)
        //        {
        //            Console.WriteLine("Service error: " + innerEx.Message);
        //            return StatusCode(500, new { error = "Service error: " + innerEx.Message });
        //        }

        //        return Ok(hostDto);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Controller error: " + ex.Message);
        //        return StatusCode(500, new { error = ex.Message });
        //    }
        //}
        [Consumes("multipart/form-data")]
        [HttpPost("AddVerifications")] // ✅ Add explicit route
        public async Task<IActionResult> AddVerifications([FromForm] HostVerificationDTO? hostDto = null)
        {
            Console.WriteLine("=== AddVerifications endpoint hit! ===");

            try
            {
                // ✅ Better null check
                if (hostDto == null)
                {
                    Console.WriteLine("❌ hostDto is null");
                    return BadRequest(new { error = "No data received" });
                }

                Console.WriteLine("✅ DTO received:");
                Console.WriteLine($"Type: {hostDto.Type}");
                Console.WriteLine($"Doc1: {hostDto.DocumentUrl1?.FileName} ({hostDto.DocumentUrl1?.Length} bytes)");
                Console.WriteLine($"Doc2: {hostDto.DocumentUrl2?.FileName} ({hostDto.DocumentUrl2?.Length} bytes)");
                Console.WriteLine($"SubmittedAt: {hostDto.SubmittedAt}");

                // ✅ Validate files
                if (hostDto.DocumentUrl1 == null || hostDto.DocumentUrl2 == null)
                {
                    Console.WriteLine("❌ Missing files");
                    return BadRequest(new { error = "Both document files are required" });
                }

                // ✅ Validate file sizes
                if (hostDto.DocumentUrl1.Length == 0 || hostDto.DocumentUrl2.Length == 0)
                {
                    Console.WriteLine("❌ Empty files");
                    return BadRequest(new { error = "Files cannot be empty" });
                }

                var result = await _ihostVerification.AddHostVerification(hostDto);
                Console.WriteLine("✅ Verification added successfully");

                return Ok(new
                {
                    message = "Verification submitted successfully",
                    verificationId = result.Id
                });
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"❌ Validation error: {ex.Message}");
                return BadRequest(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"❌ Auth error: {ex.Message}");
                return Unauthorized(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"❌ Business logic error: {ex.Message}");
                return Conflict(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Unexpected error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }
        //[HttpPut("{id}/approve")]
        ////[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> ApproveVerification(int id, [FromBody] AdminNotesDto? adminNotesDto = null)
        //{
        //    try
        //    {
        //        string adminNotes = adminNotesDto?.AdminNotes;
        //        var updatedVerification = await _ihostVerification.ApproveHostVerification(id, adminNotes);

        //        return Ok(new
        //        {
        //            message = "Host verification approved successfully",
        //            verification = updatedVerification
        //        });
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(new { error = ex.Message });
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(new { error = ex.Message });
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        return BadRequest(new { error = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { error = "Internal server error", message = ex.Message });
        //    }
        //}
        // PUT: api/admin/hostverification/{id}/approve
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveVerification(int id, [FromBody] AdminNotesDto adminNotesDto = null)
        {
            try
            {
                var adminNotes = adminNotesDto?.AdminNotes;
                var updatedVerification = await _ihostVerification.ApproveHostVerification(id, adminNotes);

                // Return a simplified response to avoid circular references
                return Ok(new
                {
                    message = "Host verification approved successfully and host is now verified",
                    verificationId = updatedVerification.Id,
                    hostId = updatedVerification.HostId,
                    status = updatedVerification.Status,
                    verifiedAt = updatedVerification.VerifiedAt,
                    adminNotes = updatedVerification.AdminNotes
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}/reject")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectVerification(int id, [FromBody] AdminNotesDto? adminNotesDto = null)
        {
            try
            {
                string adminNotes = adminNotesDto?.AdminNotes;
                var updatedVerification = await _ihostVerification.RejectHostVerification(id, adminNotes);

                return Ok(new
                {
                    message = "Host verification rejected successfully",
                    verification = updatedVerification
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }



    }
}