using Microsoft.ML.Data;

namespace bnbClone_API.Models.AiModel
{
    public class BookingData
    {
        [LoadColumn(0)]
        [KeyType(count: 1000)] 
        public uint UserId { get; set; }

        [LoadColumn(1)]
        [KeyType(count: 1000)] 
        public uint PropertyId { get; set; }

        [LoadColumn(2)]
        public float Label { get; set; } = 1;
    }
}
