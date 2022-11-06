using PracaInzynierska.Models;

namespace PracaInzynierska.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                if (!context.Monuments.Any())
                {
                    context.Monuments.AddRange(new List<Monument>()
                    {
                        new Monument()
                        {
                            Name = "Ratusz Staromiejski",
                            Image = "https://www.torun.pl/sites/default/files/styles/terms_list/public/field/image/ratusz_800_1.jpg?itok=svqevHih",
                            City = new City()
                            {
                                Name = "Toruń",
                                ZipCode = "87-100"
                            },
                            Category =  new Category()
                            {
                                Name = "Budynek"
                            },
                            Descripton = "Przez wieki stanowił administracyjne i handlowe centrum miasta, w jego pobliżu odbywały się jarmarki, hołdy składane władcom, rycerskie turnieje czy nawet publiczne egzekucje skazańców."
                        },
                        new Monument()
                        {
                            Name = "Pomnik Adama Mickiewicza",
                            Image = "https://www.zabytkikrakowa.com.pl/wp-content/uploads/mickiewicz-resized.jpg",
                            City = new City()
                            {
                                Name = "Kraków",
                                ZipCode ="30-000"
                            },
                            Category =  new Category()
                            {
                                Name = "Pomnik"
                            },
                            Descripton = "Pomnik stanął na Rynku w 1898 roku, w wyniku konkursu na pomnik wieszcza ogłoszonego w związku z setną rocznicą jego urodzin."
                        }
                    }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
