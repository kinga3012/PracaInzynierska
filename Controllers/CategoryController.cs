using Microsoft.AspNetCore.Mvc;
using PracaInzynierska.Data;
using PracaInzynierska.Interfaces;
using PracaInzynierska.Models;
using PracaInzynierska.Repository;

namespace PracaInzynierska.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> categories = await _categoryRepository.GetAll();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);
            _categoryRepository.Add(category);
            return RedirectToAction("Index");
        }


    }
}
