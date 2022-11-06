using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using PracaInzynierska.Interfaces;
using PracaInzynierska.Models;
using System.Diagnostics;

namespace PracaInzynierska.Controllers
{
    public class MonumentController : Controller
    {
        private readonly IMonumentRepository _monumentRepository;
        private readonly ICityRepository _cityRepository;
        private readonly ICategoryRepository _categoryRepository;
        
        public MonumentController(IMonumentRepository monumentRepository, ICategoryRepository categoryRepository, ICityRepository cityRepository)
        {
            _monumentRepository = monumentRepository;
            _categoryRepository = categoryRepository;
            _cityRepository = cityRepository;
        }
       
        public async Task<IActionResult> Index()
        {
            IEnumerable<Monument> monuments = await _monumentRepository.GetAll();
            return View(monuments);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Monument? monument = await _monumentRepository.GetByIdAsync(id);
            if(monument != null)
            return View(monument);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepository.GetAll();
            var cities = await _cityRepository.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewBag.Cities = new SelectList(cities, "Id", "Name");
            return View();
            //Monument monument = new Monument();

            //IEnumerable<Category> result = await _categoryRepository.GetAll();
            //List<Category> categories = new List<Category>(result);

            //IEnumerable<City> result2 = await _cityRepository.GetAll();
            //List<City> cities = new List<City>(result2);

            //MonumentCreateModel vm = new MonumentCreateModel(monument, categories, cities);

            //return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Monument monument)
        {
            monument.Category = await _categoryRepository.GetByIdAsync(monument.CategoryId);
            monument.City = await _cityRepository.GetByIdAsync(monument.CityId);
            if (ModelState.IsValid)
            {
                _monumentRepository.Add(monument);
                _monumentRepository.Save();
                return RedirectToAction("Index");
            }
            var categories = await _categoryRepository.GetAll();
            var cities = await _cityRepository.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewBag.Cities = new SelectList(cities, "Id", "Name");
            return View();
        }
    }
}
