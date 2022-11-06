using System.ComponentModel.DataAnnotations;

namespace PracaInzynierska.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(6)]
        public string ZipCode { get; set; }
    }
}
