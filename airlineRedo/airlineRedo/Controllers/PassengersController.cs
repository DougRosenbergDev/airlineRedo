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
    public class PassengersController : ControllerBase
    {
        private readonly AirlineRedoDbContext _context;

        public PassengersController(AirlineRedoDbContext context)
        {
            _context = context;
        }

        // GET: api/<Passengers>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Passenger>>> GetPassengers()
        {
            if (_context.Passengers == null)
            {
                return NotFound();
            }
            return await _context.Passengers.ToListAsync();
        }

        // GET api/<Passengers>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Passenger>> GetPassenger(int id)
        {
            if (_context.Passengers == null)
            {
                return NotFound();
            }
            var passenger = await _context.Passengers.FindAsync(id);

            if (passenger == null)
            {
                return NotFound();
            }
            var bookings = await _context.Flights.Where(p => p.Bookings.Where(op => op.PassengerId == passenger.Id).Any()).ToListAsync();
            var tickets = await _context.Bookings.Where(p => p.PassengerId == passenger.Id).ToListAsync();
            if (tickets == null)
            {
                return NotFound();
            }
            List<string> confirmationNumbers = new List<string>();
            foreach(Booking booking in tickets)
            {
                confirmationNumbers.Append(booking.ConfirmationNumber);
            }

            var passengerDto = new PassengerDetailsDTO
            {
                Id = passenger.Id,
                Name = passenger.Name,
                Job = passenger.Job,
                Email = passenger.Email,
                Age = passenger.Age,
                Bookings = bookings,
                ConfirmationNumber = confirmationNumbers
            };

            return Ok(passengerDto);
        }

        // PUT api/<PassengersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPassenger(int id, Passenger passenger)
        {
            if (id != passenger.Id)
            {
                return BadRequest();
            }

            _context.Entry(passenger).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PassengerExists(id))
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

        // POST api/<Passengers>
        [HttpPost]
        public async Task<ActionResult<Passenger>> PostPassenger(PassengerDTO passengerDto)
        {
            if (_context.Passengers == null)
            {
                return Problem("Entity set 'AirlineServiceDbContext.Passengers'  is null.");
            }

            var passenger = new Passenger(passengerDto);
            _context.Passengers.Add(passenger);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPassenger", new { id = passenger.Id }, passenger);
        }

        // DELETE api/<Passengers>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePassenger(int id)
        {
            if (_context.Passengers == null)
            {
                return NotFound();
            }
            var passenger = await _context.Passengers.FindAsync(id);

            if (passenger == null)
            {
                return NotFound();
            }

            _context.Passengers.Remove(passenger);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PassengerExists(int id)
        {
            return (_context.Passengers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
