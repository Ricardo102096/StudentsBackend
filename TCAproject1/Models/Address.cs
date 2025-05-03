using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace TCAproject1.Models
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        [StringLength(100)]
        public string? AddressLine { get; set; } 

        [StringLength(45)]
        public string? City { get; set; }

        [StringLength(10)]
        public string? ZipPostCode { get; set; }

        [StringLength(45)]
        public string? State { get; set; }

        [JsonIgnore]
        public List<Student> Students { get; set; } = new();
    }

}
