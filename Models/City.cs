using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PracaInzynierska.Models
{
    [DataContract]
    public class City
    {
        [Key]
        [DataMember]
        public int Id { get; set; }
        [MaxLength(100)]
        [DataMember]
        public string Name { get; set; }
        [MaxLength(6)]
        [DataMember]
        public string ZipCode { get; set; }
    }
}
