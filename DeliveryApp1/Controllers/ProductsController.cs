using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryApp1.Models;
using DeliveryApp1.ModelsDTO;

namespace DeliveryApp1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DeliveryAPIContext _context;

        public ProductsController(DeliveryAPIContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductsDtoRead>>> GetProducts()
        {
            var productsDtoRead = new List<ProductsDtoRead>();
            var products = await _context.Products.ToListAsync();

            foreach(var product in products)
            {
                var manufacturer = await _context.Manufacturers.FindAsync(product.Id);
                productsDtoRead.Add(
                    new ProductsDtoRead()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        ManufacturerId = manufacturer.Id,
                        Manufacturer = manufacturer!.Name
                    });
            }
            return productsDtoRead;
          /*
          if (_context.Products == null)
          {
              return NotFound();
          }
            return await _context.Products.ToListAsync();
          */
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductsDtoRead>> GetProduct(int id)
        {/*
          if (_context.Products == null)
          {
              return NotFound();
          }
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
            */
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            var manufacturer = await _context.Manufacturers.FindAsync(product.Id);
            var productsDtoRead = new ProductsDtoRead
            {
                Id = product.Id,
                Name = product.Name,
                ManufacturerId = manufacturer.Id,
                Manufacturer = manufacturer!.Name
            };
            return productsDtoRead;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            if (!IsUnique(product.Name, product.ManufacturerId))
                return Problem("Такий товар вже існує.");

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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
        /*
        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
          if (_context.Products == null)
          {
              return Problem("Entity set 'DeliveryAPIContext.Products'  is null.");
          }
            if (!IsUnique(product.Name, product.ManufacturerId))
                return Problem("Такий товар вже існує.");
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }
        */

        
        public async Task<ActionResult<Product>> PostProduct(ProductsDtoWrite productsDtoWrite)
        {
            
            //if (_context.Products == null)
            //{
            //    return Problem("Entity set 'DeliveryAPIContext.Products'  is null.");
            //}
            
            var manufacturer = await _context.Manufacturers.FindAsync(productsDtoWrite.ManufacturerId);
            if (manufacturer is null) return NotFound("Bad manufacturer id");

            var product1 = new Product()
            {
                Manufacturer = manufacturer,
                ManufacturerId = productsDtoWrite.ManufacturerId,
                Name = productsDtoWrite.Name
            };

            if (!IsUnique(product1.Name, product1.ManufacturerId))
                return Problem("Такий товар вже існує.");
            _context.Products.Add(product1);
            await _context.SaveChangesAsync();

            return RedirectToAction("GetProduct", new { id = product1.Id });
        }
        
        /*
        bool IsUnique(string name, int manufacturerid)
        {
            var a = (from product in _context.Products
                     where product.Name == name && product.ManufacturerId == manufacturerid
                     select product).ToList();
            if (a.Count == 0) { return true; }
            return false;
        }
        */

        bool IsUnique(string name, int manufacturerid)
        {
            var a = (from product in _context.Products
                     where product.Name == name && product.ManufacturerId == manufacturerid
                     select product).ToList();
            if (a.Count == 0) { return true; }
            return false;
        }
        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
