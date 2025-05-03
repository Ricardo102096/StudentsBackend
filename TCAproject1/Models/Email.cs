using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TCAproject1.Enums;
namespace TCAproject1.Models
{
    public class Email
    {
        [Key]
        [StringLength(100)]
        public string EmailAddress { get; set; } = string.Empty;

        [ForeignKey("Student")]
        public int StudentId { get; set; }

        [Required]
        public EmailType EmailType { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        [JsonIgnore]
        public Student? Student { get; set; }
    }
}
