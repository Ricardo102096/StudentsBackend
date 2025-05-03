using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TCAproject1.Data;
using TCAproject1.Models;

namespace TCAproject1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AddressController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAll()
        {
            return await _context.Addresses.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> Get(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
                return NotFound();

            return address;
        }
        [HttpGet("{id}/with-students")]
        public async Task<ActionResult<Address>> GetAddressWithStudents(int id)
        {
            var address = await _context.Addresses
                .Include(a => a.Students)
                .FirstOrDefaultAsync(a => a.AddressId == id);

            if (address == null)
                return NotFound();

            return address;
        }
        [HttpPost]
        public async Task<ActionResult<Address>> Create(Address address)
        {
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Address updatedAddress)
        {
            if (id != updatedAddress.AddressId)
                return BadRequest("AddressId in URL does not match body");

            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
                return NotFound();

            address.AddressLine = updatedAddress.AddressLine;
            address.City = updatedAddress.City;
            address.State = updatedAddress.State;
            address.ZipPostCode = updatedAddress.ZipPostCode;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
                return NotFound();

            var students = await _context.Students
                .Where(s => s.AddressId == id)
                .ToListAsync();

            foreach (var student in students)
            {
                student.AddressId = null;
            }

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
