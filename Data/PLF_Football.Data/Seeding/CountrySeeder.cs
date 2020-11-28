namespace PLF_Football.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using PLF_Football.Data.Models;
    using PLF_Football.Services;

    public class CountrySeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var countriesAll = new List<Country>();

            foreach (var club in dbContext.Clubs)
            {
                if (dbContext.Countries.Any())
                {
                    return;
                }

                var countryScraperService = serviceProvider
                    .GetRequiredService<ICountryScraperService>();

                var countriesByClub = await countryScraperService.AddPlayersCountry(club);

                foreach (var country in countriesByClub)
                {
                    if (!countriesAll.Any(x => x.Name == country.Name))
                    {
                        countriesAll.Add(country);
                    }
                }
            }

            foreach (var country in countriesAll)
            {
                await dbContext.Countries.AddAsync(country);
            }
        }
    }
}
