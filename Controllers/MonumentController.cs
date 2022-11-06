using Microsoft.AspNetCore.Mvc;
using PracaInzynierska.Interfaces;
using PracaInzynierska.Models;

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
            Monument monument = await _monumentRepository.GetByIdAsync(id);
            return View(monument);
        }
        public async Task<IActionResult> Create()
        {
            Monument monument = new Monument();

            IEnumerable<Category> result = await _categoryRepository.GetAll();
            List<Category> categories = new List<Category>(result);

            IEnumerable<City> result2 = await _cityRepository.GetAll();
            List<City> cities = new List<City>(result2);

            MonumentCreateModel vm = new MonumentCreateModel(monument, categories, cities);

            return View(vm);
        }
        [HttpPost]
        public async Task<ActionResult> Create(MonumentCreateModel vm)
        {
            if (!ModelState.IsValid)
                return View();
            else 
            {
                Category category = await _categoryRepository.GetByIdAsync(vm.selectedCategoryId);
                vm.Monument.CategoryId = category.Id;
                City city = await _cityRepository.GetByIdAsync(vm.selectedCityId);
                vm.Monument.CityId = city.Id;

                //Monument newMonument = new Monument();
                //newMonument.Name = vm.Monument.Name;
                //newMonument.Image = vm.Monument.Image;
                //newMonument.CityId = vm.Monument.CityId;
                //newMonument.CategoryId = vm.Monument.CategoryId;
                //newMonument.Descripton = vm.Monument.Descripton;

                _monumentRepository.Add(vm.Monument);
                _monumentRepository.Save();
                return View();
            }

        }
    }
}
