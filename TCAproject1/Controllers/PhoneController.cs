using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TCAproject1.Data;
using TCAproject1.Models;

namespace TCAproject1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PhoneController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("by-student/{studentId}")]
        public async Task<ActionResult<IEnumerable<Phone>>> GetByStudent(int studentId)
        {
            return await _context.Phones
                .Where(p => p.StudentId == studentId)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Phone>> Create(Phone phone)
        {
            bool phoneExists = _context.Phones.Any(p => p.PhoneNumber == phone.PhoneNumber);
            if (phoneExists)
            {
                ModelState.AddModelError("phoneNumber", "PhoneNumber already exists");
                return BadRequest(ModelState);
            }

            phone.CreatedOn = DateTime.Now;
            phone.UpdatedOn = DateTime.Now;
            _context.Phones.Add(phone);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var phone = await _context.Phones.FindAsync(id);
            if (phone == null)
                return NotFound();

            _context.Phones.Remove(phone);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
