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
        ////[Authorize(Roles = "Host")]
        //public async Task<IActionResult> AddVerifications([FromForm] HostVerificationDTO? hostDto = null)
        //{

        //    if (hostDto != null)
        //    {

        //        await _ihostVerification.AddHostVerification(hostDto);

        //        return Ok(hostDto);

        //    }


        //    return BadRequest(new { error = "Enter Required Data" });

        //}
        [Consumes("multipart/form-data")]
        [HttpPost]
        public async Task<IActionResult> AddVerifications([FromForm] HostVerificationDTO? hostDto = null)
        {

            Console.WriteLine("AddVerifications endpoint hit!");

            try
            {
                Console.WriteLine("AddVerifications endpoint hit!");

                if (hostDto == null)
                    return BadRequest(new { error = "Enter Required Data" });

                Console.WriteLine("DTO received:");
                Console.WriteLine("Type: " + hostDto.Type);
                Console.WriteLine("Doc1: " + hostDto.DocumentUrl1?.FileName);
                Console.WriteLine("Doc2: " + hostDto.DocumentUrl2?.FileName);

                // Try-catch the service layer too
                try
                {
                    await _ihostVerification.AddHostVerification(hostDto);
                }
                catch (Exception innerEx)
                {
                    Console.WriteLine("Service error: " + innerEx.Message);
                    return StatusCode(500, new { error = "Service error: " + innerEx.Message });
                }

                return Ok(hostDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Controller error: " + ex.Message);
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("{id}/approve")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveVerification(int id, [FromBody] AdminNotesDto? adminNotesDto = null)
        {
            try
            {
                string adminNotes = adminNotesDto?.AdminNotes;
                var updatedVerification = await _ihostVerification.ApproveHostVerification(id, adminNotes);

                return Ok(new
                {
                    message = "Host verification approved successfully",
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


        //[Consumes("multipart/form-data")]
        //[HttpPut("{id}")]
        //public async Task<IActionResult> EditVerifications(int id, [FromForm] HostVerificationDTO host)
        //{

        //    if (id != null && host != null)
        //    {

        //        await _ihostVerification.EditHostVerification(id, host);
        //        return Ok(host);
        //    }

        //    return BadRequest(new { error = "Enter Required Data and u must Enter ID Field" });

        //}

    }
}