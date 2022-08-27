using airlineRedo.Models;

namespace airlineRedo.DTO
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public int PassengerId { get; set; }
        public string ConfirmationNumber { get; set; }
    }
}
