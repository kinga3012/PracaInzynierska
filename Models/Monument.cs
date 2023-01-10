using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PracaInzynierska.Models
{
    [DataContract]
    public class Monument
    {
        [Key]
        [DataMember]
        public int Id { get; set; }
        [MaxLength(100)]
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Image { get; set; }
        [ForeignKey("City")]
        [DataMember]
        public int CityId { get; set; }
        public virtual City? City { get; set; }
        [MaxLength(200)]
        [DataMember]
        public string Descripton { get; set; }
        [ForeignKey("Category")]
        [DataMember]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}
