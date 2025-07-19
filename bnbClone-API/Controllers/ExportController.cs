using bnbClone_API.Data;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace bnbClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExportController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("bookings-to-csv")]
        public IActionResult ExportBookingsToCsv()
        {
            var data = _context.Bookings
                .Select(b => new
                {
                    UserId = b.GuestId,
                    PropertyId = b.PropertyId,
                    Label = 1
                })
                .Distinct()
                .ToList();

            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream);
            using var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture));

            // Write header
            csvWriter.WriteField("UserId");
            csvWriter.WriteField("PropertyId");
            csvWriter.WriteField("Label");
            csvWriter.NextRecord();

            // Write rows
            foreach (var row in data)
            {
                csvWriter.WriteField(row.UserId);
                csvWriter.WriteField(row.PropertyId);
                csvWriter.WriteField(row.Label);
                csvWriter.NextRecord();
            }

            streamWriter.Flush();
            var result = memoryStream.ToArray();
            return File(result, "text/csv", "booking_training_data.csv");
        }
    }
}
