using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TCAproject1.Controllers.Requests;
using TCAproject1.Data;
using TCAproject1.Models;

namespace TCAproject1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetAll()
        {
            return await _context.Students
                .Include(s => s.Phones)
                .Include(s => s.Emails)
                .Include(s => s.Address)
                .ToListAsync();
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
            bool error = false;
            //Verify email exists
            bool correoExistente = _context.Emails.Any(x => x.EmailAddress == newStudent.Email);
            if (correoExistente)
            {
                ModelState.AddModelError("email", "The email does exist");
                error = true;
            }
            //verify phone exists
            bool telefonoExistente = _context.Phones.Any(x => x.PhoneNumber == newStudent.PhoneNumber);
            if (telefonoExistente)
            {
                ModelState.AddModelError("phoneNumber", "The phoneNumber does exist");
                error = true;
            }
            if(error)
            {
                return BadRequest(ModelState);
            }
            if (ModelState.IsValid)
            {
                Student student = new Student();
                Phone phone = new Phone();
                Email email = new Email();
                //structure student model
                student.FirstName = newStudent.FirstName;
                student.MiddleName = newStudent.MiddleName;
                student.LastName = newStudent.LastName;
                student.Gender = newStudent.Gender;
                if(newStudent.AddressId != null)
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
            return BadRequest(ModelState);
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
