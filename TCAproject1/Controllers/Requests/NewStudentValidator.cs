using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TCAproject1.Data;

namespace TCAproject1.Controllers.Requests
{
    public class NewStudentValidator : AbstractValidator<NewStudent>

    {
        public NewStudentValidator(AppDbContext context)
        {
            RuleFor(s => s.FirstName)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(45);

            RuleFor(s => s.LastName)
                .NotEmpty().WithMessage("El apellido es obligatorio")
                .MaximumLength(45);

            RuleFor(s => s.Email)
                .NotEmpty().WithMessage("El correo es obligatorio")
                .EmailAddress().WithMessage("Formato de correo inválido")
                .MustAsync(async (email, cancel) =>
                    !await context.Emails.AnyAsync(e => e.EmailAddress == email, cancel))
                .WithMessage("Este correo ya está registrado");

            RuleFor(s => s.PhoneNumber)
                .NotEmpty().WithMessage("El teléfono es obligatorio")
                .Matches(@"^\d{7,15}$").WithMessage("El teléfono debe tener entre 7 y 15 dígitos");

            RuleFor(s => s.Gender)
                .IsInEnum().WithMessage("Género inválido");
        }
    }

}
