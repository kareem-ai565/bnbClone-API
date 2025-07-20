using bnbClone_API.Data;
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



    }
}
