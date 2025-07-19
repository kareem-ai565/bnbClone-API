using Microsoft.ML;

namespace bnbClone_API.Models.AiModel
{
    public class RecommendationService
    {

        private readonly MLContext _mlContext;
        private readonly ITransformer _model;
        private readonly PredictionEngine<BookingData, BookingPrediction> _predictionEngine;

        public RecommendationService(string modelPath)
        {
            _mlContext = new MLContext();
            using var stream = new FileStream(modelPath, FileMode.Open, FileAccess.Read);
            _model = _mlContext.Model.Load(stream, out _);
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<BookingData, BookingPrediction>(_model);
        }

        public float PredictScore(int userId, int propertyId)
        {
            var input = new BookingData
            {
                UserId = (uint)userId,
                PropertyId = (uint)propertyId
            };

            var prediction = _predictionEngine.Predict(input);
            return prediction.Score;
        }
    }
}
