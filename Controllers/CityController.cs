using Microsoft.AspNetCore.Mvc;
using PracaInzynierska.Data;
using PracaInzynierska.Interfaces;
using PracaInzynierska.Models;

namespace PracaInzynierska.Controllers
{
    public class CityController : Controller
    {
        private readonly ICityRepository _cityRepository;
        public CityController(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<City> cities = await _cityRepository.GetAll();
            return View(cities);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(City city)
        {
            if (!ModelState.IsValid)
                return View(city);
            _cityRepository.Add(city);
            return RedirectToAction("Index");
        }
    }
}
