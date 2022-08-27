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
    public class FlightsController : ControllerBase
    {
        private readonly AirlineRedoDbContext _context;

        public FlightsController(AirlineRedoDbContext context)
        {
            _context = context;
        }

        // GET: api/<FlightsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flight>>> GetFlights()
        {
            if (_context.Flights == null)
            {
                return NotFound();
            }
            return await _context.Flights.ToListAsync();
        }

        // GET api/<FlightsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> GetFlight(int id)
        {
            if (_context.Flights == null)
            {
                return NotFound();
            }
            var flight = await _context.Flights.FindAsync(id);

            if (flight == null)
            {
                return NotFound();
            }

            var bookings = await _context.Passengers.Where(p => p.Bookings.Where(op => op.FlightId == flight.Id).Any()).ToListAsync();

            var flightDto = new FlightDetailsDTO
            {
                Id = flight.Id,
                FlightNumber = flight.FlightNumber,
                Destination = flight.Destination,
                DepartureDateTime = flight.DepartureDateTime,
                ArrivalDateTime = flight.ArrivalDateTime,
                DepartureAirport = flight.DepartureAirport,
                ArrivalAirport = flight.ArrivalAirport,
                MaxCapacity = flight.MaxCapacity,
                Bookings = bookings
            };

            return Ok(flightDto);
        }

        // PUT api/<FlightsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlight(int id, Flight flight)
        {
            if (id != flight.Id)
            {
                return BadRequest();
            }
            _context.Entry(flight).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlightExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST api/<Flights>
        [HttpPost]
        public async Task<ActionResult<Flight>> PostFlight(FlightDTO flightDto)
        {
            if (_context.Flights == null)
            {
                return Problem("Entity set 'FlightDbContext.Flights' is null");
            }

            var flight = new Flight(flightDto);
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFlight", new { id = flight.Id }, flight);
        }
        
        // DELETE api/<Flights>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            if (_context.Flights == null)
            {
                return NotFound();
            }
            var flight = await _context.Flights.FindAsync(id);

            if (flight == null)
            {
                return NotFound();
            }

            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FlightExists(int id)
        {
            return (_context.Flights?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
