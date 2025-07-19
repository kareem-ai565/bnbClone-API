using bnbClone_API.Data;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static bnbClone_API.Models.AiModel.BookingData;

namespace bnbClone_API.Models.AiModel
{
    public class Exporter
    {
        private readonly ApplicationDbContext _context;

        public Exporter(ApplicationDbContext context)
        {
            _context = context;
        }

        // Exporter.cs
        public void ExportBookingDataToCsv(string filePath)
        {
            // Get positive samples
            var positiveSamples = _context.Bookings
                .AsNoTracking()
                .Where(b => b.Status == "Confirmed" || b.Status == "Completed")
                .Select(b => new BookingCsvRow
                {
                    UserId = b.GuestId,
                    PropertyId = b.PropertyId,
                    Label = 1f
                })
                .ToList();

            // Get all users and properties
            var allUsers = positiveSamples.Select(b => b.UserId).Distinct().ToList();
            var allProperties = _context.Properties.Select(p => p.Id).ToList();

            // Generate negative samples
            var negativeSamples = new List<BookingCsvRow>();
            var random = new Random();
            const int negativePerUser = 4;

            foreach (var userId in allUsers)
            {
                var userProperties = positiveSamples
                    .Where(b => b.UserId == userId)
                    .Select(b => b.PropertyId)
                    .ToHashSet();

                var availableProperties = allProperties
                    .Except(userProperties)
                    .OrderBy(_ => random.Next())
                    .Take(negativePerUser);

                negativeSamples.AddRange(availableProperties.Select(propertyId =>
                    new BookingCsvRow
                    {
                        UserId = userId,
                        PropertyId = propertyId,
                        Label = 0f
                    }));
            }

            // Combine and shuffle
            var allSamples = positiveSamples
                .Concat(negativeSamples)
                .OrderBy(_ => random.Next())
                .ToList();

            // Write to CSV
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(allSamples);
        }


    }
}
