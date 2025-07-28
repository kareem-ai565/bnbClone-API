using System.ComponentModel.DataAnnotations;

namespace bnbClone_API.DTOs
{
    public class BookingCreateDto
    {
        public int PropertyId { get; set; }
        //[DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        //[DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public int TotalGuests { get; set; }
        public int? PromotionId { get; set; }
    }
}
