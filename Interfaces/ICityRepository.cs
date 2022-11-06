using PracaInzynierska.Models;

namespace PracaInzynierska.Interfaces
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetAll();
        Task<City> GetByIdAsync(int id);
        bool Add(City city);
        bool Update(City city);
        bool Delete(City city);
        bool Save();
    }
}
