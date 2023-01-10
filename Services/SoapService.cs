using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PracaInzynierska.Interfaces;
using PracaInzynierska.Services;
using X.PagedList;

namespace PracaInzynierska.Models
{
    public class SoapService : ISoapService
    {
        private readonly ICityRepository _cityRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMonumentRepository _monumentRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public SoapService(ICityRepository cityRepository, ICategoryRepository categoryRepository, IMonumentRepository monumentRepository,
            UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _cityRepository = cityRepository;
            _categoryRepository = categoryRepository;
            _monumentRepository = monumentRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IEnumerable<City>> GetAllCities()
        {
            return await _cityRepository.GetAll();
        }

        public async Task<IEnumerable<Monument>> GetPageOfMonuments(int page)
        {
            return await _monumentRepository.GetAllPaged(null, null, "AZ", page, 6).ToListAsync();
        }

        public async Task<IEnumerable<Monument>> GetMonumentsFromCity(int city)
        {
            return await _monumentRepository.GetAllPaged(city, null, "AZ", 1, 6).ToListAsync();
        }

        public async Task<IEnumerable<Monument>> GetMonumentsFromCityByCategory(int city, int category)
        {
            return await _monumentRepository.GetAllPaged(city, category, "AZ", 1, 6).ToListAsync();
        }

        public async Task<IEnumerable<Monument>> GetMonumentsFromCityByCategorySorted(int city, int category, string sort)
        {
            return await _monumentRepository.GetAllPaged(city, category, sort, 1, 6).ToListAsync();
        }
        public async Task<Monument?> GetMonument(int id)
        {
            return await _monumentRepository.GetByIdAsync(id);
        }
        public async Task<Monument?> AddMonument(string name, string image, int city, int category, string description)
        {
            Monument monument = new()
            {
                Name = name,
                Image = image,
                CityId = city,
                CategoryId = category,
                Descripton = description
            };
            _monumentRepository.Add(monument);
            return await _monumentRepository.GetByIdAsync(monument.Id);
        }
        public async Task<Monument?> EditMonument(int id, string name, string image, int city, int category, string description)
        {
            Monument monument = await _monumentRepository.GetByIdAsync(id);
            monument.Name = name;
            monument.Image = image;
            monument.CityId = city;
            monument.CategoryId = category;
            monument.Descripton = description;
            _monumentRepository.Update(monument);
            return await _monumentRepository.GetByIdAsync(monument.Id);
        }

        public async Task<Monument?> DeleteMonument(int id)
        {
            Monument monument = await _monumentRepository.GetByIdAsync(id);
            _monumentRepository.Delete(monument);
            return await _monumentRepository.GetByIdAsync(id);
        }

        public async Task<UserHelper> Login(string email, string password)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, true);
                return new UserHelper(user.UserName, result.Succeeded);
            }
            return new UserHelper();
        }

        public async Task<UserRegisterHelper> Register(string email, string password)
        {
            IdentityUser tmpUser = new()
            {
                UserName = email,
                Email = email
            };
            IdentityResult result = await _userManager.CreateAsync(tmpUser, password);
            return new UserRegisterHelper(result.Errors.ToList(), result.Succeeded);
        }
    }
}
