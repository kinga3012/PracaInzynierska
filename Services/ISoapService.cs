using Microsoft.AspNetCore.Identity;
using PracaInzynierska.Services;
using System.ServiceModel;
using X.PagedList;

namespace PracaInzynierska.Models
{
    [ServiceContract]
    public interface ISoapService
    {
        [OperationContract]
        public Task<IEnumerable<City>> GetAllCities();
        [OperationContract]
        public Task<IEnumerable<Monument>> GetPageOfMonuments(int page);
        [OperationContract]
        public Task<IEnumerable<Monument>> GetMonumentsFromCity(int city);
        [OperationContract]
        public Task<IEnumerable<Monument>> GetMonumentsFromCityByCategory(int city, int category);
        [OperationContract]
        public Task<IEnumerable<Monument>> GetMonumentsFromCityByCategorySorted(int city, int category, string sort);
        [OperationContract]
        public Task<Monument?> GetMonument(int id);
        [OperationContract]
        public Task<Monument?> AddMonument(string name, string image, int city, int category, string description);
        [OperationContract]
        public Task<Monument?> EditMonument(int id, string name, string image, int city, int category, string description);
        [OperationContract]
        public Task<Monument?> DeleteMonument(int id);
        [OperationContract]
        public Task<UserHelper> Login(string email, string password);
        [OperationContract]
        public Task<UserRegisterHelper> Register(string email, string password);
    }
}
