using System.Net;
using System.Numerics;
using System.ComponentModel.DataAnnotations;
using TCAproject1.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace TCAproject1.Models
{
    [Index(nameof(LastName))]
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required, StringLength(45)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(45)]
        public string? MiddleName { get; set; }

        [StringLength(45)]
        public string FirstName { get; set; } = string.Empty;
        [ForeignKey("Address")]
        public int? AddressId { get; set; }

        [Required]
        public Gender Gender { get; set; }
        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public List<Phone> Phones { get; set; } = new();
        public List<Email> Emails { get; set; } = new();
        public Address? Address { get; set; }
    }

}
