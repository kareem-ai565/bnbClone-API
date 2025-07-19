using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using static bnbClone_API.Models.AiModel.BookingData;

namespace bnbClone_API.Models.AiModel
{
    

        public class RecommendationService
        {
            private static MLContext context = new MLContext();
            private static ITransformer model;
            private static PredictionEngine<BookingCsvRow, BookingPrediction> predictionEngine;

            public static void LoadModel(string modelPath)
            {
                DataViewSchema modelSchema;
                model = context.Model.Load(modelPath, out modelSchema);
                predictionEngine = context.Model.CreatePredictionEngine<BookingCsvRow, BookingPrediction>(model);
            }

        // RecommendationService.cs
        public static float PredictScore(int userId, int propertyId)
        {
            try
            {
                var input = new BookingCsvRow
                {
                    UserId = userId,
                    PropertyId = propertyId
                };

                var prediction = predictionEngine.Predict(input);
                return prediction.Score;
            }
            catch
            {
                return 0f; // Return 0 instead of NaN
            }
        }

        // RecommendationService.cs
        public static void TrainModel(string dataPath, string modelPath)
        {
            var context = new MLContext(seed: 1);

            // Use generic LoadFromTextFile with class mapping
            var data = context.Data.LoadFromTextFile<BookingCsvRow>(
                path: dataPath,
                hasHeader: true,
                separatorChar: ','
            );

            // Build pipeline
            var pipeline = context.Transforms.Conversion.MapValueToKey(
                    outputColumnName: "UserIdKey",
                    inputColumnName: nameof(BookingCsvRow.UserId))
                .Append(context.Transforms.Conversion.MapValueToKey(
                    outputColumnName: "PropertyIdKey",
                    inputColumnName: nameof(BookingCsvRow.PropertyId)))
                .Append(context.Recommendation().Trainers.MatrixFactorization(
                    new MatrixFactorizationTrainer.Options
                    {
                        MatrixColumnIndexColumnName = "UserIdKey",
                        MatrixRowIndexColumnName = "PropertyIdKey",
                        LabelColumnName = nameof(BookingCsvRow.Label),
                        NumberOfIterations = 30,
                        ApproximationRank = 32,
                        LearningRate = 0.1,
                        Lambda = 0.025,
                        LossFunction = MatrixFactorizationTrainer.LossFunctionType.SquareLossRegression,
                        Alpha = 0.01,
                        C = 0.0001,
                        Quiet = false
                    }));

            // Train model
            var model = pipeline.Fit(data);

            // Save model
            context.Model.Save(model, data.Schema, modelPath);
        }
    }

    }

