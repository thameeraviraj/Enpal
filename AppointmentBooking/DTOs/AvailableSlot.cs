using System.Text.Json.Serialization;

namespace AppointmentBooking.DTOs
{
    public class AvailableSlot
    {
        [JsonPropertyName("available_count")]
        public int AvailableCount { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime StartDate { get; set; }
    }
}
