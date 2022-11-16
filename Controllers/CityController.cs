using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PracaInzynierska.Data;
using PracaInzynierska.Interfaces;
using PracaInzynierska.Models;
using PracaInzynierska.Repository;

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
        public async Task<IActionResult> Edit(int id)
        {
            var city = await _cityRepository.GetByIdAsync(id);

            if (city == null)
                return View("Error");

            return View(city);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(City city)
        {
            if (city != null)
                _cityRepository.Update(city);
            else
                return View("Error");
            return RedirectToAction("Index");
        }
    }
}
