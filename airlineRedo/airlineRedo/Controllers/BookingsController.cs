using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using airlineRedo.Data;
using airlineRedo.Models;
using airlineRedo.DTO;

namespace airlineRedo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly AirlineRedoDbContext _context;

        public BookingsController(AirlineRedoDbContext context)
        {
            _context = context;
        }

        // GET: api/<Bookings>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBooking()
        {
            if (_context.Bookings == null)
            {
                return NotFound();
            }
            return await _context.Bookings
                .ToListAsync();
        }

        // no need to GET by Id

        // POST api/Bookings
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(BookingDTO bookingDto)
        {
            if (_context.Bookings == null)
            {
                return Problem("Entity set 'AirlineRedoDbContext.Booking' is null.");
            }
            var Flight = await _context.Flights.FirstOrDefaultAsync(f => f.Id == bookingDto.FlightId);
            var Passenger = await _context.Passengers.FirstOrDefaultAsync(p => p.Id == bookingDto.PassengerId);

            if (Flight == null || Passenger == null)
            {
                return Problem("Flassenger does not exist");
            }

            var booking = new Booking()
            {
                FlightId = bookingDto.FlightId,
                PassengerId = bookingDto.PassengerId,
                ConfirmationNumber = bookingDto.ConfirmationNumber,
                Flight = Flight,
                Passenger = Passenger,
            };
            _context.Bookings.Add(booking);
            Passenger.Bookings.Add(booking);
            Flight.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
        }

        // DELETE api/<BookingsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            if (_context.Bookings == null)
            {
                return NotFound();
            }
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            if (booking.Flight != null)
            {
                booking.Flight.Bookings.Remove(booking);
            }

            if (booking.Passenger != null)
            {
                booking.Passenger.Bookings.Remove(booking);
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(int idFlight, int idPassenger)
        {
            return (_context.Bookings?.Any(e => e.FlightId == idFlight && e.PassengerId == idPassenger)).GetValueOrDefault();
        }
    }
}
