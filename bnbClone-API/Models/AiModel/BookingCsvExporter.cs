using bnbClone_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System.Formats.Asn1;
using System.Globalization;

namespace bnbClone_API.Models.AiModel
{
    public class BookingCsvExporter
    {
        public static void TrainModel(string dataPath, string modelPath)
        {
            var mlContext = new MLContext();

            var data = mlContext.Data.LoadFromTextFile<BookingData>(
                path: dataPath,
                hasHeader: true,
                separatorChar: ',');

            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = nameof(BookingData.UserId),
                MatrixRowIndexColumnName = nameof(BookingData.PropertyId),
                LabelColumnName = nameof(BookingData.Label),
                NumberOfIterations = 20,
                ApproximationRank = 100
            };

            var pipeline = mlContext.Recommendation().Trainers.MatrixFactorization(options);
            var model = pipeline.Fit(data);

            mlContext.Model.Save(model, data.Schema, modelPath);
        }
    }
}
