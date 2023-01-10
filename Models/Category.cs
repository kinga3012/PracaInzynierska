using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PracaInzynierska.Models
{
    [DataContract]
    public class Category
    {
        [Key]
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}
