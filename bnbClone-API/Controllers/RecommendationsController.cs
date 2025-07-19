using bnbClone_API.Data;
using bnbClone_API.Models.AiModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bnbClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private RecommendationService _recommender =>
      new RecommendationService(Path.Combine("MLModels", "recommender.zip"));


        public RecommendationsController(ApplicationDbContext context)
        {
            _context = context;
        }



        [HttpPost("train-model")]
        public IActionResult TrainModel()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "bookingData.csv");
            BookingCsvExporter.TrainModel(path, "MLModels/recommender.zip");


            return Ok("Model trained and saved!");
        }



        [HttpGet("user/{userId}")]
        public IActionResult GetRecommendations(int userId)
        {
            var modelPath = Path.Combine("MLModels", "recommender.zip");

            if (!System.IO.File.Exists(modelPath))
                return BadRequest("Model file not found. Please train the model first.");

            var recommender = new RecommendationService(modelPath);

            var allProperties = _context.Properties.Select(p => p.Id).ToList();

            var recommended = allProperties
                .Select(propId => new
                {
                    PropertyId = propId,
                    Score = recommender.PredictScore(userId, propId)
                })
                .OrderByDescending(p => p.Score)
                .Take(5)
                .ToList();

            return Ok(recommended);
        }




    }
}
