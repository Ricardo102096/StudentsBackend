using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TCAproject1.Enums;

namespace TCAproject1.Models
{
    public class Phone
    {
        [Key]
        public int PhoneId { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }

        [Required, StringLength(30)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public PhoneType PhoneType { get; set; }

        [StringLength(5)]
        public string? CountryCode { get; set; }

        [StringLength(5)]
        public string? AreaCode { get; set; }
        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
        [JsonIgnore]

        public Student? Student { get; set; }
    }

}
