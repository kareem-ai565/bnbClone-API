using Microsoft.ML.Data;

namespace bnbClone_API.Models.AiModel
{
    public class BookingData
    {
        public class BookingCsvRow
        {
            [LoadColumn(0)]
            public int UserId { get; set; }
            


            [LoadColumn(1)]
            public int PropertyId { get; set; }
           


            [LoadColumn(2)]
            public float Label { get; set; } = 1f;

        }

    }
}
