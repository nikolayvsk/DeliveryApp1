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
    public class UsersController : ControllerBase
    {
        private readonly DeliveryAPIContext _context;

        public UsersController(DeliveryAPIContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            if (_context.Users == null)
            {
                return Problem("Entity set 'DeliveryAPIContext.Users'  is null.");
            }
            if (IsUnique(user.Login, user.PhoneNumber, user.DateBirth) == 1)
            {
                return Problem("Такий користувач вже існує.");
            }
            else if (IsUnique(user.Login, user.PhoneNumber, user.DateBirth) == 2)
            {
                return Problem("Такий користувач з таким телефоном вже існує.");
            }
            else if (IsUnique(user.Login, user.PhoneNumber, user.DateBirth) == 3)
            {
                return Problem("Цей логін вже зайнятий.");
            }
            else if (IsUnique(user.Login, user.PhoneNumber, user.DateBirth) == 4)
            {
                return Problem("Цей номер телефон вже існує.");
            }
            else

                _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
          if (_context.Users == null)
          {
              return Problem("Entity set 'DeliveryAPIContext.Users'  is null.");
          }
            if (IsUnique(user.Login, user.PhoneNumber, user.DateBirth) == 1)
            {
                return Problem("Такий користувач вже існує.");
            }
            else if (IsUnique(user.Login, user.PhoneNumber, user.DateBirth) == 2)
            {
                return Problem("Такий користувач з таким телефоном вже існує.");
            }
            else if (IsUnique(user.Login, user.PhoneNumber, user.DateBirth) == 3)
            {
                return Problem("Цей логін вже зайнятий.");
            }
            else if (IsUnique(user.Login, user.PhoneNumber, user.DateBirth) == 4)
            {
                return Problem("Цей номер телефон вже існує.");
            }
            else
                _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        int IsUnique(string login, string phonenumber, DateTime dateofBirth)
        {
            var a = (from user in _context.Users
                     where user.Login == login && user.PhoneNumber == phonenumber && user.DateBirth == dateofBirth
                     select user).ToList();
            if (a.Count != 0) { return 1; }
            var b = (from user in _context.Users
                     where user.Login == login && user.PhoneNumber == phonenumber
                     select user).ToList();
            if (b.Count != 0) { return 2; }

            var c = (from user in _context.Users
                     where user.Login == login
                     select user).ToList();
            if (c.Count != 0) { return 3; }

            var d = (from user in _context.Users
                     where user.PhoneNumber == phonenumber
                     select user).ToList();
            if (d.Count != 0) { return 4; }

            return 0;
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
