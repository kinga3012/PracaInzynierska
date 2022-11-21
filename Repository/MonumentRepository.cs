using Microsoft.EntityFrameworkCore;
using PracaInzynierska.Data;
using PracaInzynierska.Interfaces;
using PracaInzynierska.Models;
using X.PagedList;

namespace PracaInzynierska.Repository
{
    public class MonumentRepository : IMonumentRepository
    {
        private readonly ApplicationDbContext _context;
        public MonumentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Monument monument)
        {
            _context.Add(monument);
            return Save();
        }

        public bool Delete(Monument monument)
        {
            _context.Remove(monument);
            return Save();
        }

        public async Task<IEnumerable<Monument>> GetAll()
        {
            return await _context.Monuments.ToListAsync();
        }

        public IPagedList<Monument> GetAllPaged(int? city, int? category, string sort, int page, int pageSize)
        {
            if (city != null && category == null)
            {
                if (sort == "AZ")
                    return _context.Monuments.Where(monument => monument.CityId == city).OrderBy(x => x.Name).ToPagedList(page, pageSize);
                if (sort == "ZA")
                    return _context.Monuments.Where(monument => monument.CityId == city).OrderByDescending(x => x.Name).ToPagedList(page, pageSize);
            }

            if (category != null && city == null)
            {
                if (sort == "AZ")
                    return _context.Monuments.Where(monument => monument.CategoryId == category).OrderBy(x => x.Name).ToPagedList(page, pageSize);
                if (sort == "ZA")
                    return _context.Monuments.Where(monument => monument.CategoryId == category).OrderByDescending(x => x.Name).ToPagedList(page, pageSize);
            }

            if (city != null && category != null)
            {
                if (sort == "AZ")
                    return _context.Monuments.Where(monument => monument.CityId == city).Where(monument => monument.CategoryId == category).OrderBy(x => x.Name).ToPagedList(page, pageSize);
                if (sort == "ZA")
                    return _context.Monuments.Where(monument => monument.CityId == city).Where(monument => monument.CategoryId == category).OrderByDescending(x => x.Name).ToPagedList(page, pageSize);
            }
            if (sort == "ZA")
                return _context.Monuments.OrderByDescending(x => x.Name).ToPagedList(page, pageSize);
            return _context.Monuments.OrderBy(x => x.Name).ToPagedList(page, pageSize);
        }
        public async Task<Monument?> GetByIdAsync(int id)
        {
            var monument = await _context.Monuments.Include(x => x.Category).Include(y => y.City).FirstOrDefaultAsync(x => x.Id == id);
            return monument;
        }

        public async Task<IEnumerable<Monument>> GetMonumentsByCategory(string category)
        {
            return await _context.Monuments.Include(i => i.Category).Include(c => c.City).Where(c => c.Category.Name.Contains(category)).ToListAsync();
        }

        public async Task<IEnumerable<Monument>> GetMonumentsByCity(string city)
        {
            return await _context.Monuments.Include(i => i.Category).Include(c => c.City).Where(c => c.City.Name.Contains(city)).ToListAsync();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Monument monument)
        {
            _context.Update(monument);
            return Save();
        }
    }
}
