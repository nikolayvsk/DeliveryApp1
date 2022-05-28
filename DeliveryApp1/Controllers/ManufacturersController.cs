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
    public class ManufacturersController : ControllerBase
    {
        private readonly DeliveryAPIContext _context;

        public ManufacturersController(DeliveryAPIContext context)
        {
            _context = context;
        }

        // GET: api/Manufacturers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Manufacturer>>> GetManufacturers()
        {
          if (_context.Manufacturers == null)
          {
              return NotFound();
          }
            return await _context.Manufacturers.ToListAsync();
        }

        // GET: api/Manufacturers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Manufacturer>> GetManufacturer(int id)
        {
          if (_context.Manufacturers == null)
          {
              return NotFound();
          }
            var manufacturer = await _context.Manufacturers.FindAsync(id);

            if (manufacturer == null)
            {
                return NotFound();
            }

            return manufacturer;
        }

        // PUT: api/Manufacturers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutManufacturer(int id, Manufacturer manufacturer)
        {
            if (id != manufacturer.Id)
            {
                return BadRequest();
            }
            if (!IsUnique(manufacturer.Name))
                return Problem("Такий виробник вже існує.");

            _context.Entry(manufacturer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ManufacturerExists(id))
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

        // POST: api/Manufacturers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Manufacturer>> PostManufacturer(Manufacturer manufacturer)
        {
          if (_context.Manufacturers == null)
          {
              return Problem("Entity set 'DeliveryAPIContext.Manufacturers'  is null.");
          }
            if (!IsUnique(manufacturer.Name))
                return Problem("Такий виробник вже існує.");
            _context.Manufacturers.Add(manufacturer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetManufacturer", new { id = manufacturer.Id }, manufacturer);
        }

        bool IsUnique(string name)
        {
            var a = (from manufacturer in _context.Manufacturers
                     where manufacturer.Name == name
                     select manufacturer).ToList();
            if (a.Count == 0) { return true; }
            return false;
        }

        // DELETE: api/Manufacturers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteManufacturer(int id)
        {
            if (_context.Manufacturers == null)
            {
                return NotFound();
            }
            var manufacturer = await _context.Manufacturers.FindAsync(id);
            if (manufacturer == null)
            {
                return NotFound();
            }

            _context.Manufacturers.Remove(manufacturer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ManufacturerExists(int id)
        {
            return (_context.Manufacturers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
