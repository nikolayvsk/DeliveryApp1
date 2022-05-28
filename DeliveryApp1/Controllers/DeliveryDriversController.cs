using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryApp1.Models;

namespace DeliveryApp1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryDriversController : ControllerBase
    {
        private readonly DeliveryAPIContext _context;

        public DeliveryDriversController(DeliveryAPIContext context)
        {
            _context = context;
        }

        // GET: api/DeliveryDrivers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryDriver>>> GetDeliveryDrivers()
        {
          if (_context.DeliveryDrivers == null)
          {
              return NotFound();
          }
            return await _context.DeliveryDrivers.ToListAsync();
        }

        // GET: api/DeliveryDrivers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryDriver>> GetDeliveryDriver(int id)
        {
          if (_context.DeliveryDrivers == null)
          {
              return NotFound();
          }
            var deliveryDriver = await _context.DeliveryDrivers.FindAsync(id);

            if (deliveryDriver == null)
            {
                return NotFound();
            }

            return deliveryDriver;
        }

        // PUT: api/DeliveryDrivers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeliveryDriver(int id, DeliveryDriver deliveryDriver)
        {
            if (id != deliveryDriver.Id)
            {
                return BadRequest();
            }
            if (_context.DeliveryDrivers == null)
            {
                return Problem("Entity set 'DeliveryAPIContext.DeliveryDrivers'  is null.");
            }
            if (IsUnique(deliveryDriver.Login, deliveryDriver.PhoneNumber, deliveryDriver.DateBirth) == 1)
            {
                return Problem("Такий доставник вже існує.");
            }
            else if (IsUnique(deliveryDriver.Login, deliveryDriver.PhoneNumber, deliveryDriver.DateBirth) == 2)
            {
                return Problem("Такий доставник з таким телефоном вже існує.");
            }
            else if (IsUnique(deliveryDriver.Login, deliveryDriver.PhoneNumber, deliveryDriver.DateBirth) == 3)
            {
                return Problem("Цей логін вже зайнятий.");
            }
            else if (IsUnique(deliveryDriver.Login, deliveryDriver.PhoneNumber, deliveryDriver.DateBirth) == 4)
            {
                return Problem("Цей номер телефон вже існує.");
            }
            else
                _context.Entry(deliveryDriver).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeliveryDriverExists(id))
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

        // POST: api/DeliveryDrivers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DeliveryDriver>> PostDeliveryDriver(DeliveryDriver deliveryDriver)
        {
          if (_context.DeliveryDrivers == null)
          {
              return Problem("Entity set 'DeliveryAPIContext.DeliveryDrivers'  is null.");
          }
            if (IsUnique(deliveryDriver.Login, deliveryDriver.PhoneNumber, deliveryDriver.DateBirth) == 1)
            {
                return Problem("Такий доставник вже існує.");
            }
            else if (IsUnique(deliveryDriver.Login, deliveryDriver.PhoneNumber, deliveryDriver.DateBirth) == 2)
            {
                return Problem("Такий доставник з таким телефоном вже існує.");
            }
            else if (IsUnique(deliveryDriver.Login, deliveryDriver.PhoneNumber, deliveryDriver.DateBirth) == 3)
            {
                return Problem("Цей логін вже зайнятий.");
            }
            else if (IsUnique(deliveryDriver.Login, deliveryDriver.PhoneNumber, deliveryDriver.DateBirth) == 4)
            {
                return Problem("Цей номер телефон вже існує.");
            }
            else
                _context.DeliveryDrivers.Add(deliveryDriver);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDeliveryDriver", new { id = deliveryDriver.Id }, deliveryDriver);
        }

        int IsUnique(string login, string phonenumber, DateTime dateofBirth)
        {
            var a = (from deliverydriver in _context.DeliveryDrivers
                     where deliverydriver.Login == login && deliverydriver.PhoneNumber == phonenumber && deliverydriver.DateBirth == dateofBirth
                     select deliverydriver).ToList();
            if(a.Count != 0) { return 1; }
            var b = (from deliverydriver in _context.DeliveryDrivers
                     where deliverydriver.Login == login && deliverydriver.PhoneNumber == phonenumber
                     select deliverydriver).ToList();
            if(b.Count != 0) { return 2; }

            var c = (from deliverydriver in _context.DeliveryDrivers
                     where deliverydriver.Login == login
                     select deliverydriver).ToList();
            if(c.Count != 0) { return 3; }

            var d = (from deliverydriver in _context.DeliveryDrivers
                     where deliverydriver.PhoneNumber == phonenumber
                     select deliverydriver).ToList();
            if (d.Count != 0) { return 4; }

            return 0;
        }

        class Compars : IEqualityComparer<string[]>
        {
            #region IEqualityComparer<string[]> Members

            public bool Equals(string[] x, string[] y)
            {
                return x[2].Equals(y[2]);
            }

            public int GetHashCode(string[] w)
            {
                return 0;
            }

            #endregion
        }

        // DELETE: api/DeliveryDrivers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeliveryDriver(int id)
        {
            if (_context.DeliveryDrivers == null)
            {
                return NotFound();
            }
            var deliveryDriver = await _context.DeliveryDrivers.FindAsync(id);
            if (deliveryDriver == null)
            {
                return NotFound();
            }

            _context.DeliveryDrivers.Remove(deliveryDriver);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeliveryDriverExists(int id)
        {
            return (_context.DeliveryDrivers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
