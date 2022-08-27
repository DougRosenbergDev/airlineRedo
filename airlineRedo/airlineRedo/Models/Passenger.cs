using airlineRedo.DTO;

namespace airlineRedo.Models
{
    public class Passenger
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public Passenger() { }

        public Passenger(PassengerDTO passengerDTO)
        {
            Name = passengerDTO.Name;
            Job = passengerDTO.Job;
            Email = passengerDTO.Email;
            Age = passengerDTO.Age;
            Bookings = new List<Booking>();

        }
    }
}
