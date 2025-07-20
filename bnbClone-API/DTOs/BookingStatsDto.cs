namespace bnbClone_API.DTOs
{
    public class BookingStatsDto
    {
    public int TotalBookings { get; set; }
    public int ConfirmedBookings { get; set; }
    public int CancelledBookings { get; set; }
    public int PendingBookings { get; set; }
    public int CompletedBookings { get; set; }
    public int DeniedBookings { get; set; }
    }
}
