using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TCAproject1.Data;
using TCAproject1.Models;
using Microsoft.EntityFrameworkCore;

namespace TCAproject1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmailController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Email>>> GetAll()
        {
            return await _context.Emails.ToListAsync();
        }
        [HttpGet("by-student/{studentId}")]
        public async Task<ActionResult<IEnumerable<Email>>> GetByStudent(int studentId)
        {
            return await _context.Emails
                .Where(e => e.StudentId == studentId)
                .ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult<Email>> Create(Email email)
        {
            bool emailExists = _context.Emails.Any(e => e.EmailAddress == email.EmailAddress);
            if (emailExists)
            {
                ModelState.AddModelError("emailAddress", "EmailAddress already exists");
                return BadRequest(ModelState);
            }

            email.CreatedOn = DateTime.Now;
            email.UpdatedOn = DateTime.Now;
            _context.Emails.Add(email);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{emailAddress}")]
        public async Task<IActionResult> Update(string emailAddress, Email updatedEmail)
        {
            if (emailAddress != updatedEmail.EmailAddress)
                return BadRequest("EmailAddress in URL does not match body");

            var existingEmail = await _context.Emails.FindAsync(emailAddress);
            if (existingEmail == null)
                return NotFound();

            existingEmail.EmailType = updatedEmail.EmailType;
            existingEmail.StudentId = updatedEmail.StudentId;
            existingEmail.UpdatedOn = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{emailAddress}")]
        public async Task<IActionResult> Delete(string emailAddress)
        {
            var email = await _context.Emails.FindAsync(emailAddress);
            if (email == null)
                return NotFound();

            _context.Emails.Remove(email);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
