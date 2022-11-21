using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using PracaInzynierska.Interfaces;
using PracaInzynierska.Models;
using PracaInzynierska.Repository;
using System.Diagnostics;
using X.PagedList;

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

        public async Task<IActionResult> Index(string? city, string? category, string? sort, int? page = 1)
        {
            var categories = await _categoryRepository.GetAll();
            var cities = await _cityRepository.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewBag.Cities = new SelectList(cities, "Id", "Name");
            int? cityInt = null;
            int? categoryInt = null;
            if (city != null && city != "All")
                cityInt = int.Parse(city);
            if (category != null && category != "All")
                categoryInt = int.Parse(category);
            if (page != null && page < 1)
                page = 1;
            var pageSize = 3;
            IPagedList<Monument> monuments = _monumentRepository.GetAllPaged(cityInt, categoryInt, sort ?? "AZ", page ?? 1, pageSize);
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

        public async Task<IActionResult> Edit(int id)
        {
            var monument = await _monumentRepository.GetByIdAsync(id);

            if (monument == null)
                return View("Error");

            var categories = await _categoryRepository.GetAll();
            var cities = await _cityRepository.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewBag.Cities = new SelectList(cities, "Id", "Name");
            return View(monument);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Monument monument)
        {
            if(monument != null)
                _monumentRepository.Update(monument);
            else
                return View("Error");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            Monument? monument = await _monumentRepository.GetByIdAsync(id);
            return View(monument);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteMonument(int id)
        {
            Monument? monument = await _monumentRepository.GetByIdAsync(id);
            if (monument == null)
                return View("Error");
            _monumentRepository.Delete(monument);
            _monumentRepository.Save();
            return RedirectToAction("Index");
        }
    }
}
