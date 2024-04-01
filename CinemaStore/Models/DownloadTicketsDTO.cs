namespace CinemaStore.Models
{
    public class DownloadTicketsDTO
    {
        public string CinemaCenter { get; set; }
        public string HallName { get; set; }
        public string MovieTitle { get; set; }
        public string DateTime { get; set; }
        public int NumberOfTickets { get; set; }
        public int TotalValue { get; set; }
        public string BookingId { get; set; }

    }
}