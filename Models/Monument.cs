using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PracaInzynierska.Models
{
    public class Monument
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public string Image { get; set; }
        [ForeignKey("City")]
        public int CityId { get; set; }
        public City City { get; set; }
        [MaxLength(200)]
        public string Descripton { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }

    public class MonumentCreateModel 
    {

        public Monument Monument { get; set; }
        public  SelectList selectCategories { get; set; }
        public int selectedCategoryId { get; set; }

        public SelectList selectCities { get; set; }
        public int selectedCityId { get; set; }

        public MonumentCreateModel(Monument monument, List<Category> categories, List<City> cities)
        {
            Monument = monument;
            this.selectCategories = new SelectList(categories, "Id", "Name");
            this.selectCities = new SelectList(cities, "Id", "Name");
        }
        public MonumentCreateModel() { }
    }
}
