using bnbClone_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System.Formats.Asn1;
using System.Globalization;
using static bnbClone_API.Models.AiModel.BookingData;

namespace bnbClone_API.Models.AiModel
{
    public class BookingCsvExporter
    {
        // BookingCsvExporter.cs
        public static void TrainModel(string dataPath, string modelPath)
        {
            var mlContext = new MLContext();

            // Add missing configuration
            var options = new TextLoader.Options
            {
                Columns = new[]
                {
            new TextLoader.Column(nameof(BookingCsvRow.UserId), DataKind.Int32, 0),
            new TextLoader.Column(nameof(BookingCsvRow.PropertyId), DataKind.Int32, 1),
            new TextLoader.Column(nameof(BookingCsvRow.Label), DataKind.Single, 2)
        },
                HasHeader = true,
                Separators = new[] { ',' }
            };

            var loader = mlContext.Data.CreateTextLoader(options);
            var data = loader.Load(dataPath);

            var trainerOptions = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = nameof(BookingCsvRow.UserId),
                MatrixRowIndexColumnName = nameof(BookingCsvRow.PropertyId),
                LabelColumnName = nameof(BookingCsvRow.Label),
                NumberOfIterations = 20,
                ApproximationRank = 100
            };

            var pipeline = mlContext.Recommendation().Trainers.MatrixFactorization(trainerOptions);
            var model = pipeline.Fit(data);

            mlContext.Model.Save(model, data.Schema, modelPath);
        }
    }
}
