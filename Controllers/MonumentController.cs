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

        //public async Task<IActionResult> Index()
        //{
        //    IEnumerable<Monument> monuments = await _monumentRepository.GetAll();
        //    return View(monuments);
        //}
        public IActionResult Index(string filter, string sort, int? page = 1)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sort) ? "name_desc" : "";

            if (page != null && page < 1)
            {
                page = 1;
            }
            var pageSize = 3;

            var monuments = _monumentRepository.GetAllPaged(page ?? 1, pageSize);

            return View(monuments);
        }
        //[HttpPost]
        //public async Task<IActionResult> Index(string category)
        //{
        //    IEnumerable<Monument> monuments = await _monumentRepository.GetMonumentsByCategory(category);
        //    return View(monuments);
        //}
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
