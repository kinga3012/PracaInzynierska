using Microsoft.EntityFrameworkCore;
using PracaInzynierska.Data;
using PracaInzynierska.Interfaces;
using PracaInzynierska.Models;
using System.ComponentModel;

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

        public async Task<Monument> GetByIdAsync(int id)
        {
            return await _context.Monuments.Include(i => i.Category).Include(c => c.City).FirstOrDefaultAsync(i => i.Id == id);
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
            throw new NotImplementedException();
        }
    }
}
