using Microsoft.EntityFrameworkCore;
using PracaInzynierska.Data;
using PracaInzynierska.Interfaces;
using PracaInzynierska.Models;

namespace PracaInzynierska.Repository
{
    public class CityRepository : ICityRepository
    {
        private readonly ApplicationDbContext _context;
        public CityRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(City city)
        {
            _context.Add(city);
            return Save();
        }

        public bool Delete(City city)
        {
            _context.Remove(city);
            return Save();
        }

        public async Task<IEnumerable<City>> GetAll()
        {
            return await _context.Cities.ToListAsync();
        }

        public async Task<City> GetByIdAsync(int id)
        {
            return await _context.Cities.FirstOrDefaultAsync(i => i.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(City city)
        {
            throw new NotImplementedException();
        }
    }
}
