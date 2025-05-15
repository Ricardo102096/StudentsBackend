using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TCAproject1.Controllers.Requests;
using TCAproject1.Data;
using TCAproject1.Helpers;
using TCAproject1.Models;

namespace TCAproject1.Controllers.V2
{
    [Route("api/v2/[Controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IValidator<NewStudent> _validator;
        private readonly ILogger<StudentController> _logger;
        public StudentController(AppDbContext context, IValidator<NewStudent> validator, ILogger<StudentController> logger)
        {
            _context = context;
            _validator = validator;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<PagedResult<Student>>> GetAll(
       [FromQuery] int page = 1,
       [FromQuery] int pageSize = 10,
       [FromQuery] string? search = null)
        {
            var query = _context.Students
                .Include(s => s.Address)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s =>
                    s.FirstName.Contains(search) ||
                    s.LastName.Contains(search) ||
                    (s.MiddleName != null && s.MiddleName.Contains(search))
                );
            }

            var result = await query.ToPagedResultAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> Get(int id)
        {
            var student = await _context.Students
                .Include(s => s.Phones)
                .Include(s => s.Emails)
                .Include(s => s.Address)
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
                return NotFound();

            return student;
        }

        [HttpPost]
        public async Task<ActionResult<Student>> Create(NewStudent newStudent)
        {
            var result = await _validator.ValidateAsync(newStudent);
            if (!result.IsValid)
            {
                return BadRequest(result.ToDictionary());
            }
            Student student = new Student();
            Phone phone = new Phone();
            Email email = new Email();
            //structure student model
            student.FirstName = newStudent.FirstName;
            student.MiddleName = newStudent.MiddleName;
            student.LastName = newStudent.LastName;
            student.Gender = newStudent.Gender;
            if (newStudent.AddressId != null)
            {
                var AddressData = await _context.Addresses.FindAsync(newStudent.AddressId);
                student.AddressId = newStudent.AddressId;
                student.Address = AddressData;
            }
            student.CreatedOn = DateTime.Now;
            student.UpdatedOn = DateTime.Now;
            _context.Add(student);
            await _context.SaveChangesAsync();
            //structure phone model
            phone.PhoneNumber = newStudent.PhoneNumber;
            phone.PhoneType = newStudent.PhoneType;
            phone.CountryCode = newStudent.CountryCode;
            phone.AreaCode = newStudent.AreaCode;
            phone.CreatedOn = DateTime.Now;
            phone.UpdatedOn = DateTime.Now;
            phone.StudentId = student.StudentId;
            phone.Student = student;
            _context.Add(phone);
            //structure email model
            email.EmailAddress = newStudent.Email;
            email.EmailType = newStudent.EmailType;
            email.CreatedOn = DateTime.Now;
            email.UpdatedOn = DateTime.Now;
            email.StudentId = student.StudentId;
            email.Student = student;
            _context.Add(email);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Student updatedStudent)
        {
            if (id != updatedStudent.StudentId)
                return BadRequest();

            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return NotFound();

            student.FirstName = updatedStudent.FirstName;
            student.LastName = updatedStudent.LastName;
            student.MiddleName = updatedStudent.MiddleName;
            student.Gender = updatedStudent.Gender;
            student.UpdatedOn = DateTime.Now;
            student.AddressId = updatedStudent.AddressId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students
                .Include(s => s.Emails)
                .Include(s => s.Phones)
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
                return NotFound();

            _context.Emails.RemoveRange(student.Emails);
            _context.Phones.RemoveRange(student.Phones);
            _context.Students.Remove(student);

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
