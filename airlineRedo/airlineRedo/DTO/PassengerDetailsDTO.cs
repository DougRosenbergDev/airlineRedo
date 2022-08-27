using airlineRedo.Models;

namespace airlineRedo.DTO
{
    public class PassengerDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public List<Flight> Bookings { get; set; }
        public List<string> ConfirmationNumber { get; set; }
    }
}
