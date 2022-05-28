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
    public class OrdersController : ControllerBase
    {
        private readonly DeliveryAPIContext _context;

        public OrdersController(DeliveryAPIContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrdersDtoRead>>> GetOrders()
        {
            var ordersDtoRead = new List<OrdersDtoRead>();
            var orders = await _context.Orders.ToListAsync();

            foreach (var order in orders)
            {
                var user = await _context.Users.FindAsync(order.Id);
                var deliverydriver = await _context.DeliveryDrivers.FindAsync(order.Id);
                var product = await _context.Products.FindAsync(order.Id);
                ordersDtoRead.Add(
                    new OrdersDtoRead()
                    {
                        Id = order.Id,
                        DateSale = order.DateSale,

                        UserId = user.Id,
                        User = user!.Login,

                        ProductId = product.Id,
                        Product = product!.Name,

                        DeliveryDriverId = deliverydriver.Id,
                        DeliveryDriver = deliverydriver!.Login
                    });
            }
            return ordersDtoRead;
            /*
          if (_context.Orders == null)
          {
              return NotFound();
          }
            return await _context.Orders.ToListAsync();
            */
            //return await _context.Orders.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrdersDtoRead>> GetOrder(int id)
        {
          /*
          if (_context.Orders == null)
          {
              return NotFound();
          }
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
          */
          var order = await _context.Orders.FindAsync(id);
          if(order == null) return NotFound();
            var user = await _context.Users.FindAsync(order.Id);
            var deliverydriver = await _context.DeliveryDrivers.FindAsync(order.Id);
            var product = await _context.Products.FindAsync(order.Id);
            var ordersDtoRead = new OrdersDtoRead
                {
                    Id = order.Id,
                    DateSale = order.DateSale,

                    UserId = user.Id,
                    User = user!.Login,

                    ProductId = product.Id,
                    Product = product!.Name,

                    DeliveryDriverId = deliverydriver.Id,
                    DeliveryDriver = deliverydriver!.Login
                };
            return ordersDtoRead;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(OrdersDtoWrite ordersDtoWrite)
        {
            var user = await _context.Users.FindAsync(ordersDtoWrite.UserId);
            var deliverydriver = await _context.DeliveryDrivers.FindAsync(ordersDtoWrite.DeliveryDriverId);
            var product = await _context.Products.FindAsync(ordersDtoWrite.ProductId);

            if (user is null) return NotFound("Bad user id");
            if (deliverydriver is null) return NotFound("Bad deliverydriver id");
            if (product is null) return NotFound("Bad product id");

            var order1 = new Order()
            {
                User = user,
                DeliveryDriver = deliverydriver,
                Product = product,
                UserId = ordersDtoWrite.UserId,
                DeliveryDriverId = ordersDtoWrite.DeliveryDriverId,
                ProductId = ordersDtoWrite.ProductId
            };

            _context.Orders.Add(order1);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetOrder", new { id = order.Id }, order);
            return RedirectToAction("GetOrder", new { id = order1.Id });
        }
        /*
        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
          if (_context.Orders == null)
          {
              return Problem("Entity set 'DeliveryAPIContext.Orders'  is null.");
          }
            if (!IsUnique(order.DateSale, order.UserId, order.DeliveryDriverId, order.ProductId))
                return Problem("Таке замовлення вже існує.");
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }
        */

        bool IsUnique(DateTime datesale, int userId, int deliverydriverid, int productid)
        {
            var a = (from order in _context.Orders
                     where order.DateSale == datesale && order.UserId == userId && order.DeliveryDriverId == deliverydriverid && order.ProductId == productid
                     select order).ToList();
            if (a.Count == 0) { return true; }
            return false;
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
