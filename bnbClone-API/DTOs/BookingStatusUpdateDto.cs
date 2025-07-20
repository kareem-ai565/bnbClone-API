using bnbClone_API.Models;
using System.Text.Json.Serialization;

namespace bnbClone_API.DTOs
{
    public class BookingStatusUpdateDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BookingStatus Status { get; set; }
    }
}
