using System.ComponentModel.DataAnnotations;
using TCAproject1.Enums;
using TCAproject1.Models;

namespace TCAproject1.Controllers.Requests
{
    public class NewStudent
    {
        [Required(ErrorMessage = "Insert the FirstName from the student")]
        [StringLength(45)]
        public string FirstName { get; set; } = string.Empty;
        [StringLength(45)]
        public string? MiddleName {  get; set; } 
        [Required(ErrorMessage = "Insert the LastName from the student")]
        [StringLength(45)]
        public string LastName { get; set; } = string.Empty;
        public Gender Gender { get; set; } = Gender.Other;
        [Required(ErrorMessage ="Email is required")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        public EmailType EmailType { get; set; } = EmailType.Personal;
        [Required(ErrorMessage = "PhoneNumber is required")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(30)]
        public string PhoneNumber {  get; set; } = string.Empty;
        public PhoneType PhoneType { get; set; } = PhoneType.Mobile;
        [StringLength(5)]
        public string CountryCode {  get; set; } = string.Empty;
        [StringLength(5)]
        public string AreaCode {  get; set; } = string.Empty;
        public int? AddressId { get; set; } 
    }
}
