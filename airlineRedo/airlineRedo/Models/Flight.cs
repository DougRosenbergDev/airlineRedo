using airlineRedo.DTO;

namespace airlineRedo.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureDateTime { get; set; }
        public DateTime ArrivalDateTime { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public int MaxCapacity { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public Flight() { }
        public Flight(FlightDTO flightDTO)
        {
            FlightNumber = flightDTO.FlightNumber;
            Destination = flightDTO.Destination;
            DepartureDateTime = flightDTO.DepartureDateTime;
            ArrivalDateTime = flightDTO.ArrivalDateTime;
            ArrivalAirport = flightDTO.ArrivalAirport;
            DepartureAirport = flightDTO.DepartureAirport;
            ArrivalAirport = flightDTO.ArrivalAirport;
            MaxCapacity = flightDTO.MaxCapacity;
            Bookings = new List<Booking>();
        }
    }
}
