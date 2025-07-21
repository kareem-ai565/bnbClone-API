using bnbClone_API.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace bnbClone_API.DTOs
{
    public class BookingResponseDto
    {
        public int Id { get; set; }
        public string PropertyTitle { get; set; }
        public string PropertyAddress { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public string GuestName { get; set; }
        public int TotalGuests { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]

        public BookingStatus Status { get; set; }
        public string CheckInStatus { get; set; }
        public string CheckOutStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public int? PromotionId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
