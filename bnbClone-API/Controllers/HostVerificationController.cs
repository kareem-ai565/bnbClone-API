using bnbClone_API.DTOs;
using bnbClone_API.Models;
using bnbClone_API.Services.Impelementations;
using bnbClone_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace bnbClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostVerificationController : ControllerBase
    {
        private readonly IhostVerificationService ihostVerification;

        public HostVerificationController(IhostVerificationService ihostVerification)
        {
            this.ihostVerification = ihostVerification;
        }



        [HttpGet]
        public async Task<IActionResult> GetAllVerifications()
        {
            IEnumerable<HostVerification> hostVerifications = await ihostVerification.GetAllHostVerification();

            if (hostVerifications != null)
            {
                return Ok(hostVerifications);
            }
            else
            {
                return NotFound(new { error = "No Verifications" });
            }

        }


        [Consumes("multipart/form-data")]
        [HttpPost]
        public async Task<IActionResult> AddVerifications([FromForm] HostVerificationDTO host)
        {

            if (host != null)
            {

                await ihostVerification.AddHostVerification(host);

                return Ok(host);

            }


            return BadRequest(new { error = "Enter Required Data" });

        }


        [Consumes("multipart/form-data")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditVerifications(int id, [FromForm] HostVerificationDTO host)
        {

            if (id != null && host != null)
            {

                await ihostVerification.EditHostVerification(id, host);
                return Ok(host);
            }

            return BadRequest(new { error = "Enter Required Data and u must Enter ID Field" });

        }




    }
}
