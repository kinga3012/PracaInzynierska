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
            /* Zwracam listę wszystkich miast */
            return await _cityRepository.GetAll(); 
        }

        public async Task<IEnumerable<Monument>> GetPageOfMonuments(int page)
        {
            /* Zwracam wybraną w parametrze page stronę zabytków. 
             Nie filtruję, sortowanie domyślne, 6 - liczba zabytków na stronie (tak założyłam). */
            return await _monumentRepository.GetAllPaged(null, null, "AZ", page, 6).ToListAsync();
        }

        public async Task<IEnumerable<Monument>> GetMonumentsFromCity(int city)
        {
            /* Zwracam listę zabytków z miasta określonego w parametrze city (id miasta).
            Nie filtruję kategorii, sortowanie domyślne, 6 - liczba zabytków na stronie (tak założyłam), 1 - pierwsza strona. */
            return await _monumentRepository.GetAllPaged(city, null, "AZ", 1, 6).ToListAsync();
        }

        public async Task<IEnumerable<Monument>> GetMonumentsFromCityByCategory(int city, int category)
        {
            /* Zwracam listę zabytków z miasta określonego w parametrze city (id miasta) 
            i o katergorii z parametru category (id kategorii). 
            Sortowanie domyślne, 6 - liczba zabytków na stronie (tak założyłam), 1 - pierwsza strona. */
            return await _monumentRepository.GetAllPaged(city, category, "AZ", 1, 6).ToListAsync();
        }

        public async Task<IEnumerable<Monument>> GetMonumentsFromCityByCategorySorted(int city, int category, string sort)
        {
            /* Zwracam listę zabytków z miasta określonego w parametrze city (id miasta) 
            i o katergorii z parametru category (id kategorii) oraz z określonym w parametrze sort sortowaniem (AZ lub ZA). 
            6 - liczba zabytków na stronie (tak założyłam), 1 - pierwsza strona. */
            return await _monumentRepository.GetAllPaged(city, category, sort, 1, 6).ToListAsync();
        }
        public async Task<Monument?> GetMonument(int id)
        {
            /* Zwracam zabytek o określonym id */
            return await _monumentRepository.GetByIdAsync(id);
        }
        public async Task<Monument?> AddMonument(string name, string image, int city, int category, string description)
        {
            /* Dodaję do bazy nowy wpis z zabytkiem. */
            Monument monument = new() // Tworzę nowy zabytek na podstawie danych z argumentów metody.
            {
                Name = name,
                Image = image,
                CityId = city,
                CategoryId = category,
                Descripton = description
            };
            _monumentRepository.Add(monument); // Dodaję do bazy.
            return await _monumentRepository.GetByIdAsync(monument.Id);  // Zwracam zabytek o id nowostworzonego zabytku, w celu sprawdzenia czy został dodany do bazy.
        }

        public async Task<Monument?> EditMonument(int id, string name, string image, int city, int category, string description)
        {
            /* Edytuję istniejący zabytek */
            Monument monument = await _monumentRepository.GetByIdAsync(id); // Pobieram do zmiennej zabytek o wskazanym id.
            if (monument == null || name == null || name == "" || image == null || image == "" ||
                await _cityRepository.GetByIdAsync(city) == null || await _categoryRepository.GetByIdAsync(category) == null ||
                description == null || description == "") // Sprawdzam czy argumenty nie zostały puste - wtedy nie będę edytować.
                return null;

            monument.Name = name; // Przypisuję nowe dane.
            monument.Image = image;
            monument.CityId = city;
            monument.CategoryId = category;
            monument.Descripton = description;

            _monumentRepository.Update(monument); // Aktulizuję wpis.
            return await _monumentRepository.GetByIdAsync(monument.Id); // Zwracam zabytek o id edytowanego zabytku, w celu sprawdzenia czy został zmieniony.
        }

        public async Task<Monument?> DeleteMonument(int id)
        {
            /* Usuwanie zabytku o określonym id */
            Monument monument = await _monumentRepository.GetByIdAsync(id); // Pobieram zabytek z bazy.
            if (monument != null) // Jeśli istnieje taki zabytek to go usuwam.
                _monumentRepository.Delete(monument);
            return await _monumentRepository.GetByIdAsync(id); // Zwracam zabytek o id usuniętego zabytku, oczekuję że zabytek nie zostanie wyszukany.
        }

        public async Task<UserLoginHelper> Login(string email, string password)
        {
            /* Logowanie */
            IdentityUser user = await _userManager.FindByEmailAsync(email); // Tworzę instancję klasy IdentityUser, wyszukując użytkownika o podanym mailu.
            if (user != null) // Jeśli użytkownik istnieje to sprawdzam czy jest poprawne hasło
            {
                SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, true);
                return new UserLoginHelper(user.UserName, result.Succeeded); // Zwracam instancję klasy pomocniczej i wartość logiczną czy udało się zalogować
                /* Wystąpił problem ze zwróceniem IdentityUser, gdyż nie mogłam dostać się do wnętrza tej klasy (bo to część frameworka Identity)
                aby nadać klasie atrybut [DataContract] a składowym [DataMember].*/
            }
            return new UserLoginHelper(); // Zwracam instancję z domyślnymi parametrami UserName = "" i LoginSuccess = false
        }

        public async Task<UserRegisterHelper> Register(string email, string password)
        {
            /* Rejestracja */
            IdentityUser tmpUser = new() // Tworzę instancję IdentityUser o odpowiednim mailu i loginie (domyślnie także nazwa jest emailem w tym frameworku)
            {
                UserName = email,
                Email = email
            };
            IdentityResult result = await _userManager.CreateAsync(tmpUser, password); // Tworzę konto dla użytkownika user
            return new UserRegisterHelper(result.Errors.ToList(), result.Succeeded);
            /* Zwracam ewentualną listę błędów (np. za krótkie hasło, zły format maila itp.)
             oraz informację czy udało się zalogować. Jak wyżej była potrzebna klasa pomocnicza. */
        }
    }
}
