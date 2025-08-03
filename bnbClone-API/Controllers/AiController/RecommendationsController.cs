using bnbClone_API.Data;
using bnbClone_API.DTOs;
using bnbClone_API.Models.AiModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bnbClone_API.Controllers.AiController
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationsController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public RecommendationsController(
            ApplicationDbContext context,
            IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpPost("train-model")]
        public IActionResult TrainRecommendationModel()
        {
            try
            {
                var exporter = new Exporter(_context);
                string dataDir = Path.Combine(_env.ContentRootPath, "MLData");
                string csvPath = Path.Combine(dataDir, "bookingData.csv");

                Directory.CreateDirectory(dataDir);
                exporter.ExportBookingDataToCsv(csvPath);

                if (new FileInfo(csvPath).Length == 0)
                    return BadRequest("No booking data available for training");

                string modelPath = Path.Combine(_env.ContentRootPath, "MLModels", "recommender.zip");
                Directory.CreateDirectory(Path.GetDirectoryName(modelPath));

                RecommendationService.TrainModel(csvPath, modelPath);
                return Ok("Model trained successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Training failed: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRecommendations(int userId)
        {
            try
            {
                string modelPath = Path.Combine(_env.ContentRootPath, "MLModels", "recommender.zip");

                if (!System.IO.File.Exists(modelPath))
                    return BadRequest("Model not found. Train model first");

                RecommendationService.LoadModel(modelPath);

                var propertyIds = await _context.Properties
                    .Select(p => p.Id)
                    .ToListAsync();

                var recommendations = propertyIds
                    .AsParallel()
                    .Select(propertyId => new
                    {
                        PropertyId = propertyId,
                        Score = RecommendationService.PredictScore(userId, propertyId)
                    })
                    .Where(p => !float.IsNaN(p.Score))
                    .OrderByDescending(r => r.Score)
                    .Take(3)
                    .ToList();

                return Ok(recommendations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Recommendation failed: {ex.Message}");
            }
        }



        [HttpPost("export-csv")]
        public IActionResult ExportToCsv()
        {
            var exporter = new Exporter(_context);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Data", "bookingData.csv");
            exporter.ExportBookingDataToCsv(path);

            return Ok("Data exported successfully to: " + path);
        }








        [HttpPost("recommend")]
        public async Task<IActionResult> Recommend([FromBody] UserPreferencesDto prefs)
        {
            var properties = await _context.Properties
                .Select(p => new {
                    p.Id,
                    p.Title,
                    p.City,
                    p.PricePerNight,
                    p.MaxGuests,
                    p.PropertyType,
                }).ToListAsync();

            var scored = properties.Select(p => new
            {
                Property = p,
                Score =
                    (p.City.Equals(prefs.PreferredCity, StringComparison.OrdinalIgnoreCase) ? 3 : 0) +
                    (p.PricePerNight <= prefs.BudgetForNight ? 2 : 0) +
                    (p.MaxGuests >= prefs.MaxGuest ? 2 : 0) +
                    (p.PropertyType.Equals(prefs.PropertyType, StringComparison.OrdinalIgnoreCase) ? 3 : 0)
            })
            .OrderByDescending(x => x.Score)
            .ToList();

            if (!scored.Any(x => x.Score > 0))
            {
                scored = properties.Select(p => new
                {
                    Property = p,
                    Score = 0
                })
                .Take(10)
                .ToList();
            }

            var result = scored.Select(x => new
            {
                x.Property.Id,
                x.Property.Title,
                x.Property.City,
                x.Property.PricePerNight,
                x.Property.MaxGuests,
                x.Property.PropertyType,
                Reason = x.Score > 0 ? GenerateReason(x.Property, prefs) : "Suggested based on availability",
                x.Score
            });

            return Ok(result);
        }

        private string GenerateReason(object property, UserPreferencesDto prefs)
        {
            var type = property.GetType().GetProperty("Type")?.GetValue(property)?.ToString() ?? string.Empty;
            var city = property.GetType().GetProperty("City")?.GetValue(property)?.ToString() ?? string.Empty;
            var price = Convert.ToDecimal(property.GetType().GetProperty("PricePerNight")?.GetValue(property) ?? 0);
            var guests = Convert.ToInt32(property.GetType().GetProperty("MaxGuests")?.GetValue(property) ?? 0);

            List<string> reasons = new();

            if (city.Equals(prefs.PreferredCity, StringComparison.OrdinalIgnoreCase))
                reasons.Add("Located in your preferred city");
            if (price <= prefs.BudgetForNight)
                reasons.Add("Fits your budget");
            if (guests >= prefs.MaxGuest)
                reasons.Add("Can host your group size");
            if (type.Equals(prefs.PropertyType, StringComparison.OrdinalIgnoreCase))
                reasons.Add("Matches your preferred property type");

            return string.Join(", ", reasons);
        }


    }
}
