using System.ComponentModel.DataAnnotations;

namespace bnbClone_API.DTOs
{
    public class BookingSearchDto
    {
        public string Status { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }
        public int? GuestId { get; set; }
        public int? PropertyId { get; set; }
    }
}
