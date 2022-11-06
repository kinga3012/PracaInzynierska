using PracaInzynierska.Models;

namespace PracaInzynierska.Interfaces
{
    public interface IMonumentRepository
    {
        Task<IEnumerable<Monument>> GetAll();
        Task<Monument> GetByIdAsync(int id);
        Task<IEnumerable<Monument>> GetMonumentsByCity(string city);
        Task<IEnumerable<Monument>> GetMonumentsByCategory(string category);
        bool Add(Monument monument);
        bool Update(Monument monument);
        bool Delete(Monument monument);
        bool Save();
    }
}
