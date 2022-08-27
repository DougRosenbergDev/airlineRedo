using System.Text.Json.Serialization;

namespace airlineRedo.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public int PassengerId { get; set; }
        [JsonIgnore]
        public virtual Flight Flight { get; set; }
        public virtual Passenger Passenger { get; set; }
        public string ConfirmationNumber { get; set; }
    }
}
